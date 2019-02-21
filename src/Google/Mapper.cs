﻿using System;
using System.Collections.Generic;
using Repositories.Health.Models;
using Utils;

namespace Google
{
    public class Mapper : IMapper
    {
        private readonly ILogger _logger;

        public Mapper(ILogger logger)
        {
            _logger = logger;
        }

        //public Run MapRowToRun(IList<object> row)
        //{
        //    var date = DateTime.Parse((string)row[0]);
        //    var m = int.Parse((string)row[1]);
        //    var time = TimeSpan.Parse((string)row[2]);

        //    return new Run
        //    {
        //        CreatedDate = date,
        //        Metres = m,
        //        Time = time
        //    };
        //}

        //public Ergo MapRowToErgo(IList<object> row)
        //{
        //    var date = DateTime.Parse((string)row[0]);
        //    var m = int.Parse((string)row[1]);
        //    var time = TimeSpan.Parse((string)row[2]);

        //    return new Ergo
        //    {
        //        CreatedDate = date,
        //        Metres = m,
        //        Time = time
        //    };
        //}

        public AlcoholIntake MapRowToAlcoholIntake(IList<object> row)
        {
            var date = DateTime.Parse((string)row[0]);
            var units = Double.Parse((string)row[1]);

            return new AlcoholIntake
            {
                CreatedDate = date,
                Units = units,

            };
        }

        public Exercise MapRowToExerise(IList<object> row)
        {
            var date = DateTime.Parse((string)row[0]);
            var metres = int.Parse((string)row[1]);
            
            var description = (string)row[3];

            var row4 = (string) row[4];

            _logger.LogMessageAsync("THIS IS THE VALUE FROM TEH SHEET : " + row4);

            var totalSeconds = int.Parse((string)row[4]);

            return new Exercise
            {
                CreatedDate = date,
                Metres = metres,
                TotalSeconds = totalSeconds,
                Description = description
            };
        }
    }
}