using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuppGamesBack.Data;
using SuppGamesBack.Models.DTOs;

namespace SuppGamesBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnnotationsController : ControllerBase
    {
        private readonly IAnnotationRepository _annotationRepository;
        private readonly IFavoriteGameRepository _favoriteGameRepository;

        private int? GetCurrentUser()
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userIdString, out var userId))
            {
                return userId;
            }

            return null;
        }

        public AnnotationsController (IAnnotationRepository annotationRepository, IFavoriteGameRepository favoriteGameRepository)
        {
            _annotationRepository = annotationRepository;
            _favoriteGameRepository = favoriteGameRepository;
        }


        [HttpPost]
        [Authorize]

        public async Task<IActionResult> AddAnnotation([FromBody] CreateAnnotationDTO annotationDTO)
        {
            var userIdFromToken = GetCurrentUser();

            if (userIdFromToken == null)
            {
                return Unauthorized();
            }

            var favoriteGame = await _favoriteGameRepository.GetByIdAsync(annotationDTO.FavoriteGameId);

            if (favoriteGame == null)
            {
                return NotFound("O jogo favorito especificado não foi encontrado.");
            }

            if (favoriteGame.UserId != userIdFromToken)
            {
                return Forbid("Você não tem permissões para adicionar anotações neste jogo.");
            }

            var newAnnotation = new Anot
            {
                Text = annotationDTO.Text,
                FavoriteGameId = annotationDTO.FavoriteGameId,
                CreateDateAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow,
                Excluded = false
            };

            var createdAnnotation = await _annotationRepository.AddAsync(newAnnotation);

            return CreatedAtAction("GetAnnotationById", new { id = createdAnnotation.Id }, createdAnnotation);
        }

        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetAllAnnotations()
        {
            var userIdFromToken = GetCurrentUser();

            if(userIdFromToken == null)
            {
                return Unauthorized();
            }

            var anots = await _annotationRepository.GetAllByUserId(userIdFromToken.Value);

            var anotDto = anots.Select(anot => new AnnotationResponseDTO
            {
                Id = anot.Id,
                FavoriteGameId = anot.FavoriteGameId,
                Text = anot.Text,
                CreatedDateAt = anot.CreateDateAt,
                LastUpdateAt = anot.LastUpdatedAt,
            }).ToList();

            return Ok(anotDto);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateAnnotation(int id, [FromBody] UpdateAnnotationDTO updateAnnotationDto)
        {
            if (id != updateAnnotationDto.Id)
            {
                return BadRequest("O ID da rota não corresponde ao ID da anotação.");
            }

            var userIdFromToken = GetCurrentUser();

            if (userIdFromToken == null)
            {
                return Unauthorized();
            }

            var annotationToUpdate = await _annotationRepository.GetByIdAsync(id);

            if (annotationToUpdate == null)
            {
                return NotFound("Anotação não encontrada.");
            }

            var favoriteGame = await _favoriteGameRepository.GetByIdAsync(annotationToUpdate.FavoriteGameId);

            if(favoriteGame.UserId != userIdFromToken.Value)
            {
                return Forbid("ACESSO NEGADO!");
            }

            annotationToUpdate.Text = updateAnnotationDto.Text;
            annotationToUpdate.LastUpdatedAt = DateTime.Now;

            await _annotationRepository.UpdateAsync(annotationToUpdate);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]

        public async Task<IActionResult> DeleteAnnotation(int id)
        {
            var userIdFromToken = GetCurrentUser();

            if (userIdFromToken == null)
            {
                return Unauthorized();
            }

            var annotationToDelete = await _annotationRepository.GetByIdAsync(id);

            if (annotationToDelete == null)
            {
                return NotFound("Anotação não encontrada.");
            }

            var favoriteGame = await _favoriteGameRepository.GetByIdAsync(id);

            if (favoriteGame.UserId != userIdFromToken)
            {
                return Forbid("ACESSO NEGADO!");
            }

            await _annotationRepository.DeleteAsync(id);

            return NoContent();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetAnnotationById(int id)
        {
            var userFromIdToken = GetCurrentUser();

            if (userFromIdToken == null)
            {
                return Unauthorized();
            }

            var annotation = await _annotationRepository.GetByIdAsync(id);
            
            if (annotation == null)
            {
                return NotFound();
            }

            var favoriteGame = await _favoriteGameRepository.GetByIdAsync(annotation.FavoriteGameId);

            if(favoriteGame.UserId != userFromIdToken)
            {
                return Forbid();
            }

            var responseDto = new AnnotationResponseDTO
            {
                Id = annotation.Id,
                FavoriteGameId = annotation.FavoriteGameId,
                Text = annotation.Text,
                CreatedDateAt = annotation.CreateDateAt,
                LastUpdateAt = annotation.LastUpdatedAt,
            };

            return Ok(responseDto);

        }
    }
}
