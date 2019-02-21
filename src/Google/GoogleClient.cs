using System;
using System.Collections.Generic;
using Repositories.Health.Models;
using Utils;

namespace Google
{
    public class GoogleClient : IGoogleClient
    {
        private readonly IConfig _config;
        private readonly ISheetMapper _sheetMapper;
        private readonly IMapper _mapper;


        public GoogleClient(IConfig config, ISheetMapper sheetMapper, IMapper mapper)
        {
            _config = config;
            _sheetMapper = sheetMapper;
            _mapper = mapper;
        }

        public List<AlcoholIntake> GetAlcoholIntakes()
        {
            return _sheetMapper.Get<AlcoholIntake>(_config.AlcoholSpreadsheetId, "Sheet1!B2:F", _mapper.MapRowToAlcoholIntake);
        }

        //public List<Ergo> GetErgos()
        //{
        //    return _sheetMapper.Get<Ergo>(_config.RowSpreadsheetId, "Sheet1!A2:C", _mapper.MapRowToErgo);
        //}

        public List<Exercise> GetExercises()
        {
            return _sheetMapper.Get<Exercise>(_config.ExerciseSpreadsheetId, "Sheet1!A2:E", _mapper.MapRowToExerise);
        }

        //public List<Run> GetRuns()
        //{
        //    return _sheetMapper.Get<Run>(_config.RunSpreadsheetId, "Sheet1!A2:C", _mapper.MapRowToRun);
        //}

    }
}