using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicAPI.Data;
using MusicAPI.Helpers;
using MusicAPI.Models;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MusicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        private ApiDbContext _dbContext;
        public AlbumsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
            public async Task<IActionResult> Post([FromForm] Album album)
            {
                var imageUrl = await FileHelper.UploadImage(album.Image);
                album.ImageUrl = imageUrl;
                await _dbContext.Albums.AddAsync(album);
                await _dbContext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created);
            }
        [HttpGet]
            public async Task<IActionResult> GetAlbums(int? pageNumber, int? pageSize)
            {
            int currentPageNumber = pageNumber ?? 1;
            int currentPageSize = pageSize ?? 5;

            var albums = await (from album in _dbContext.Albums
                                     select new
                                     {
                                         Id = album.Id,
                                         Name = album.Name,
                                         ImageUrl = album.ImageUrl
                                     }).ToListAsync();
                return Ok(albums.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageNumber));
            }
        [HttpGet("[action]")]
            public async Task<ActionResult> AlbumDetails(int albumId)
            {
                var albumDetails = await _dbContext.Albums.Where(a => a.Id == albumId).Include(a => a.Songs).ToListAsync();
                return Ok(albumDetails);
            }
    }
}
