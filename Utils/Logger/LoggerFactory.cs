using Amazon;
using Amazon.CloudWatchLogs;
using Amazon.Runtime;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.AwsCloudWatch;
using Serilog.Sinks.Elasticsearch;
using System;

namespace Utils.Logger
{
    public static class LoggerFactory
    {
        public static LoggerConfiguration BuildEmptyLoggerConfiguration()
        {
            return new LoggerConfiguration();
        }

        public static ILogger CreateLogger(LoggerConfiguration loggerConfiguration, LogEventLevel minimumLogEventLevel)
        {
            return loggerConfiguration
                .MinimumLevel.Is(minimumLogEventLevel)
                .CreateLogger();
        }

        public static LoggerConfiguration AppendFileLogger(this LoggerConfiguration logger, string logFilePath, int retainedFileCountLimit = 14)
        {
            return logger
                .WriteTo.RollingFile(pathFormat: logFilePath, formatter: new CompactJsonFormatter(), retainedFileCountLimit: retainedFileCountLimit);
        }

        public static LoggerConfiguration AppendConsoleLogger(this LoggerConfiguration logger)
        {
            return logger
                .WriteTo.Console(formatter: new CompactJsonFormatter());
        }

        public static LoggerConfiguration AppendAwsCloudwatchLogger(this LoggerConfiguration logger, string logGroupName, LogEventLevel minimumLogEvent, string accessKey, string securityKey)
        {
            // customer formatter
            var formatter = new CompactJsonFormatter();

            // options for the sink defaults in https://github.com/Cimpress-MCP/serilog-sinks-awscloudwatch/blob/master/src/Serilog.Sinks.AwsCloudWatch/CloudWatchSinkOptions.cs
            var options = new CloudWatchSinkOptions
            {
                LogGroupName = logGroupName,
                TextFormatter = formatter,
                MinimumLogEventLevel = minimumLogEvent,
                LogStreamNameProvider = new DefaultLogStreamProvider(),
                RetryAttempts = 5
            };

            var credentials = new BasicAWSCredentials(accessKey, securityKey);

            // setup AWS CloudWatch client
            var client = new AmazonCloudWatchLogsClient(credentials, RegionEndpoint.EUWest1);

            return logger
                .WriteTo.AmazonCloudWatch(options, client);
        }

        public static LoggerConfiguration AppendElasticsearchLogger(this LoggerConfiguration logger, string uri, string indexName)
        {
            var options = new ElasticsearchSinkOptions(new Uri(uri))
            {
                IndexFormat = $"{indexName}-{0:yyyy.MM}",
            };

            return logger
                .WriteTo.Elasticsearch(options);
        }
    }
}
