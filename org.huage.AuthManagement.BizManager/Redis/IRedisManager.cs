﻿using StackExchange.Redis;

namespace org.huage.AuthManagement.BizManager.Redis;

public interface IRedisManager
{
    Task<bool> SetIfNotExists(string key, string value, TimeSpan? expiry = null);
    
    Task DelayDeleteKey(string key);
    
    Task<IDatabase> SwitchDataBase(int db);
    
    Task<bool> HashExists(string redisKey, string hashField);
    
    Task<Dictionary<string,T>> HashGetAllAsync<T>(string redisKey);

    Task<List<T>> HGetAllValue<T>(string redisKey);
    
    Task<List<string>> HGetAllKeys(string redisKey);    
    
    Task<bool> HashSet(string redisKey, string hashField, string value);
    
    Task<bool> HashSet<T>(string redisKey, string hashField, T value);

    Task<string> HGet(string redisKey,string key);

    Task<bool> UpdateExpire(string redisKey,string key);
    
    Task<bool> HashDeleteFiled(string redisKey,string filed);

    Task<bool> DelKey(string redisKey);
    /// <summary>
    /// 缓存过期时间
    /// </summary>
    int TimeOut { set; get; }

   
    /// <summary>
    /// 从缓存中移除指定键的缓存值
    /// </summary>
    /// <param name="key">缓存键</param>
    void Remove(string key);
    
    /// <summary>
    /// 将指定键的对象添加到缓存中
    /// </summary>
    /// <param name="key">缓存键</param>
    /// <param name="data">缓存值</param>
    
    /// <summary>
    /// 判断key是否存在
    /// </summary>
    bool Exists(string key);
}