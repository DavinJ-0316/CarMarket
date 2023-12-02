using ActionService.DTOs;
using ActionService.Entities;
using AuctionService.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ActionService.Controller;

[ApiController] // checks for validation (for example [required]), 
// if the request does not meet validation,it does not even let the request goto the endpoint
// the framework is going to reject the request

// ControllerBase Objact transforms to JSON format
// ApiController gives abilities to make data validation easir
[Route("api/auctions")]
public class AuctionsController : ControllerBase
{
    private readonly AuctionDbContext _context;  // convension for  private readonly
    private readonly IMapper _mapper;

    // every time a new reques comes in, the AuctionsController will be instantiate as well as the context and mapper
    // IMapper an Interface we get from automapper
    public AuctionsController(AuctionDbContext context, IMapper mapper)
    {
        _context = context; // after this context been injected, we can use readonly context in the rest of the class
        _mapper = mapper;
    }


    [HttpGet]
    public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions(string date) {
        var query = _context.Auctions.OrderBy(x => x.Item.Make).AsQueryable();
        // query的类型在用了OrderBy之后变成了： IOrderedQueryable<Auction> query 
        // 所以需要把类型转换回去queryable的类型否则无法query

        if(!string.IsNullOrEmpty(date)) {  
            query = query.Where(x => x.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);

        }

        return await query.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider).ToListAsync();
        // replaced
        // var auctions = await _context.Auctions
        //     .Include(x => x.Item)
        //     .OrderBy(x => x.Item.Make)
        //     .ToListAsync(); // 真正向数据库发送请求的函数
        
        // return _mapper.Map<List<AuctionDto>>(auctions);  
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id) {
        var auction = await _context.Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id); // 真正向数据库发送请求的函数

        if(auction == null) return NotFound();

        return _mapper.Map<AuctionDto>(auction);
    }

    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto) {
        var auction = _mapper.Map<Auction>(auctionDto);
        // TODO: add current user as seller (should not allow anyone to create auction, need authentication)
        auction.Seller = "test";

        _context.Auctions.Add(auction); //这里Add有叠加之意, context类似于react的state存在memory之中

        var result = await _context.SaveChangesAsync() > 0;

        if(!result) return BadRequest("Could not save changes to the DB");

        return CreatedAtAction(nameof(GetAuctionById), 
            new {auction.Id}, _mapper.Map<AuctionDto>(auction));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto) {
        var auction = await _context.Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id);

        if(auction == null) return NotFound();

        // ToDo: check seller === username; (authentication)
        // auction.Item.Make = updateAuctionDto.Make !=null ? updateAuctionDto.Make : auction.Item.Make;
        auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
        auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
        auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
        auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
        auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;

        var result = await _context.SaveChangesAsync() > 0;

        if(!result) return BadRequest("Could not save changes to the DB");

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuction(Guid id) {
        var auction = await _context.Auctions.FindAsync(id);

        if (auction == null) return NotFound();

        // ToDo: check seller == username

        _context.Auctions.Remove(auction);
        var result = await _context.SaveChangesAsync() >0;

        if(!result) return BadRequest("Could not update DB");

        return Ok();
    }

}
