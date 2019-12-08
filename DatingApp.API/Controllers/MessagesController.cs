using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.DTOs;
using DatingApp.Domain.Shared;
using DatingApp.Domain.Interfaces;
using DatingApp.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DatingApp.API.Helpers;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        public MessagesController(IDatingRepository repo, IMapper mapper)
        {
            this._mapper = mapper;
            this._repo = repo;

        }

        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int id)
        {
            if(id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromRepo =await _repo.GetMessage(id);

            if(messageFromRepo == null)
                return NotFound();

            return Ok(messageFromRepo);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser(int userId, [FromQuery]MessageParams messageParams)
        {
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            messageParams.UserId = userId;

            var messageFromRepo = await _repo.GetMessagesForUser(messageParams);

            var messages = _mapper.Map<IEnumerable<MessageToReturnDTO>>(messageFromRepo);

            Response.AddPagination(messageFromRepo.CurrentPage, messageFromRepo.PageSize, messageFromRepo.TotalCount,messageFromRepo.TotalPages);

            return Ok(messages);
        }

        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessageThread(int userId, int recipientId)
        {
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var messagesFromRepo = await _repo.GetMessagesThread(userId, recipientId);

            var messageThread = _mapper.Map<IEnumerable<MessageToReturnDTO>>(messagesFromRepo);

            return Ok(messageThread);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, MessageForCreationDTO messageForCreationDTO)
        {
             var sender = await _repo.GetUser(userId);

               if(sender.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            messageForCreationDTO.SenderId = userId;

            var recipient = await _repo.GetUser(messageForCreationDTO.RecipientId);

            if(recipient == null)
                return BadRequest("Could not find user");
            
            var message = _mapper.Map<Message>(messageForCreationDTO);

            _repo.Add(message);
            

            if(await _repo.SaveAll()){
                var messageToReturn = _mapper.Map<MessageToReturnDTO>(message);
               return CreatedAtRoute("GetMessage", new {userId = userId, id = message.Id}, messageToReturn);
            }

            throw new Exception("Creating the message failed on save");
        }
        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteMessage(int id, int userId)
        {
             if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var messagesFromRepo = await _repo.GetMessage(id);

            if(messagesFromRepo.SenderId == userId)
                messagesFromRepo.SenderDeleted = true;

            if(messagesFromRepo.RecipientId == userId)
                messagesFromRepo.RecipientDeleted = true;

            if(messagesFromRepo.SenderDeleted && messagesFromRepo.RecipientDeleted)
            _repo.Delete(messagesFromRepo);

            if(await _repo.SaveAll())
                return NoContent();

            throw new Exception("Error Deleting the message");

        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkMessageAsRead(int id, int userId)
        {

             if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var messagesFromRepo = await _repo.GetMessage(id);

            if(messagesFromRepo.RecipientId != userId)
                return Unauthorized();

            messagesFromRepo.IsRead = true;
            messagesFromRepo.DateReadAt = DateTime.Now;

            await _repo.SaveAll();

            return NoContent();
  
        }

    }
}