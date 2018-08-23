//using System;
//using System.Linq.Expressions;
//using Hangfire;
//using Migrators.Fitbit;
//using Utils;

//namespace Migrators.Hangfire
//{
//    public class HangfireUtility : IHangfireUtility
//    {
//        private readonly IFitbitMigrator _fitbitMigrator;
//        private readonly ILogger _logger;

//        public HangfireUtility(IFitbitMigrator fitbitMigrator, ILogger logger)
//        {
//            _fitbitMigrator = fitbitMigrator;
//            _logger = logger;
//        }
        
//        //public void Enqueue(Func<Task> func)
//        //{
//        //    BackgroundJob.Enqueue(() => func());
//        //}

//        //public int Enqueue<T>(Expression<Action<T>> methodCall)
//        //{
//        //    var enqueue = BackgroundJob.Enqueue(methodCall);
//        //    var queueId = int.Parse(enqueue);
//        //    return queueId;
//        //}

//        public void Enqueue(Expression<Action> methodCall)
//        {
//            BackgroundJob.Enqueue(methodCall);
//        }

//    }
//}