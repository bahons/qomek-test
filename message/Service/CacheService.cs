using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text;
using message.Models.Domain;
using System;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace message.Service;

/// <summary>
/// Кэш сервис
/// </summary>
public class CacheService
{
    private IDistributedCache _redis;

    public CacheService(IDistributedCache redis)
    { _redis = redis; }


    public async Task<List<Message>> GetMessageList(string userId)
    {
        byte[]? data = await _redis.GetAsync(userId);
        return JsonSerializer.Deserialize<List<Message>>(data);
    }


    public async Task SetMessage(Message message)
    {
        List<Message>? result = new List<Message>();
        byte[]? data = await _redis.GetAsync(message.UserId);
        if(data  == null)
        {
            result.Add(message);
            data = Encoding.UTF8.GetBytes((String)JsonSerializer.Serialize(result, JsonOption()));
            await _redis.SetAsync(message.UserId, data, GetRedisOption());
        }
        else
        {
            string scache = Encoding.UTF8.GetString(data);
            result = JsonSerializer.Deserialize<List<Message>>(scache);
            result.Add(message);
            data = Encoding.UTF8.GetBytes((String)JsonSerializer.Serialize(result, JsonOption()));
            await _redis.SetAsync(message.UserId, data, GetRedisOption());
        }
    }



    /// <summary>
    /// Options Redis
    /// </summary>
    /// <returns></returns>
    private DistributedCacheEntryOptions GetRedisOption()
    {
        var options = new DistributedCacheEntryOptions()
        .SetSlidingExpiration(TimeSpan.FromDays(1))
        .SetAbsoluteExpiration(DateTime.Now.AddDays(1));
        return options;
    }

    /// <summary>
    /// JSON Option
    /// </summary>
    /// <returns></returns>
    private JsonSerializerOptions JsonOption()
    {
        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            WriteIndented = true
        };
        return options;
    }
}

