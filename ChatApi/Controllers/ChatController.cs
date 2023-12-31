﻿using Microsoft.AspNetCore.Mvc;
using ChatApi.Acessor;
using ChatApi.Manager;
using ChatApi.Models;

namespace ChatApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly UserAcessor userAcessor;
        private readonly ChatManager _chatManager;

        public ChatController(ChatManager chatManager, UserAcessor userAcessor)
        {
            _chatManager = chatManager;
            this.userAcessor = userAcessor;
        }
        [HttpGet]
        public async Task<List<ChatModel>> GetChatGroup()
        {
            return await _chatManager.GetConversations(userAcessor.UserId);
        }
        [HttpGet("messages")]
        public async Task<List<MessageModel>> GetMessages(Guid chatId)
        {
            return await _chatManager.GetConversationMessages(chatId);
        }
        [HttpPost]
        public async Task SaveMessages(NewMessageModel model)
        {
            await _chatManager.SaveMessage(userAcessor.UserId, model);
        }
    }
}
