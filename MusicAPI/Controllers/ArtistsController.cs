﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicAPI.Data;
using MusicAPI.Helpers;
using MusicAPI.Models;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace MusicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private ApiDbContext _dbContext;
        public ArtistsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext; 
        }

        [HttpPost]
            public async Task<IActionResult> Post([FromForm] Artist artist)
            {
                var imageUrl = await FileHelper.UploadImage(artist.Image);
                artist.ImageUrl = imageUrl;
                await _dbContext.Artists.AddAsync(artist);
                await _dbContext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created);
            }
        [HttpGet]
            public async Task<IActionResult> GetArtists(int? pageNumber, int? pageSize)
            {
            int currentPageNumber = pageNumber ?? 1;
            int currentPageSize = pageSize ?? 5;

            var artists = await (from artist in _dbContext.Artists
                                     select new
                                     {
                                         Id = artist.Id,
                                         Name = artist.Name,
                                         ImageUrl = artist.ImageUrl
                                     }).ToListAsync();
                return Ok(artists.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageNumber));
            }
        [HttpGet("[action]")]
            public async Task<ActionResult> ArtistDetails(int artistId)
            {
                var artistDetails = await _dbContext.Artists.Where(a => a.Id == artistId).Include(a => a.Songs).ToListAsync();
                return Ok(artistDetails);
            }
    }
}
