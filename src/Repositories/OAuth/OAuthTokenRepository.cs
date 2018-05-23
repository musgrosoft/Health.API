using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Runtime;
using Utils;
using Amazon.Lambda.Core;

namespace Repositories.OAuth
{
    public class OAuthTokenRepository : IOAuthTokenRepository
    {
        private readonly IConfig _config;
        private readonly ILogger _logger;
        
        public OAuthTokenRepository(IConfig config, ILogger logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SaveToken(string tokenName, string tokenValue)
        {
            try
            {
                var dynamoDbClient = new AmazonDynamoDBClient(
                    new BasicAWSCredentials(_config.DyanmoDbAccessKey, _config.DynamoDbSecretKey),
                    new AmazonDynamoDBConfig
                    {
                        RegionEndpoint = RegionEndpoint.EUWest1
                    }
                );

                Table table = Table.LoadTable(dynamoDbClient, "MyTokens");

                var tokenDocument = new Document
                {
                    ["Name"] = tokenName,
                    ["Token"] = tokenValue
                };

                await table.PutItemAsync(tokenDocument );
               
            }
            catch (Exception ex)
            {
                _logger.Log(ex.ToString());
            }
        }

        public async Task<string> ReadToken(string tokenName)
        {
            try
            {
                var dynamoDbClient = new AmazonDynamoDBClient(
                    new BasicAWSCredentials(_config.DyanmoDbAccessKey, _config.DynamoDbSecretKey),
                    new AmazonDynamoDBConfig
                    {
                        RegionEndpoint = RegionEndpoint.EUWest1
                    }
                );

                Table table = Table.LoadTable(dynamoDbClient, "MyTokens");

                GetItemOperationConfig config = new GetItemOperationConfig
                {
                    AttributesToGet = new List<string> { "Name", "Token" },
                    ConsistentRead = true
                };

                Document tokenDocument = await table.GetItemAsync(tokenName, config);
                var token = tokenDocument["Token"];
                return token;
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message);
            }

            return "Unable to read token, no exception thrown";

        }
    }
    
}