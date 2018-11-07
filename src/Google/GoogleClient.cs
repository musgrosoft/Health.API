using System;
using System.Collections.Generic;
using Repositories.Models;
using Utils;

namespace Google
{
    public class GoogleClient : IGoogleClient
    {
        private readonly IConfig _config;
        private readonly ILogger _logger;
        private readonly IGoogleRowCollector _googleRowCollector;
        private readonly IMapper _mapper;


        public GoogleClient(IConfig config, ILogger logger, IGoogleRowCollector googleRowCollector, IMapper mapper)
        {
            _config = config;
            _logger = logger;
            _googleRowCollector = googleRowCollector;
            _mapper = mapper;
        }



        public List<AlcoholIntake> GetAlcoholIntakes()
        {
            var alcoholIntakes = new List<AlcoholIntake>();

            try
            {
                var rows = _googleRowCollector.GetRows(_config.AlcoholSpreadsheetId, "Sheet1!B2:F");
                if (rows != null && rows.Count > 0)
                {
                    foreach (var row in rows)
                    {
                        try
                        {
                            alcoholIntakes.Add(_mapper.MapRowToAlcoholIntake(row));
                        }
                        catch (Exception ex)
                        {
                            _logger.LogErrorAsync(ex);
                        }

                    }
                }



            }
            catch (Exception ex)
            {
                _logger.LogErrorAsync(ex);
            }

            return alcoholIntakes;
        }

        public List<Ergo> GetErgos()
        {
            var ergos = new List<Ergo>();

            try
            {
                var rows = _googleRowCollector.GetRows(_config.RowSpreadsheetId, "Sheet1!A2:C");
                if (rows != null && rows.Count > 0)
                {
                    foreach (var row in rows)
                    {
                        try
                        {
                            ergos.Add(_mapper.MapRowToErgo(row));
                        }
                        catch (Exception ex)
                        {
                            _logger.LogErrorAsync(ex);
                        }

                    }
                }



            }
            catch (Exception ex)
            {
                _logger.LogErrorAsync(ex);
            }

            return ergos;
        }

        public List<Run> GetRuns()
        {
            var runs = new List<Run>();

            try
            {
                var rows = _googleRowCollector.GetRows(_config.RunSpreadsheetId, "Sheet1!A2:C");
                if (rows != null && rows.Count > 0)
                {
                    foreach (var row in rows)
                    {
                        try
                        {
                            runs.Add(_mapper.MapRowToRun(row));
                        }
                        catch (Exception ex)
                        {
                            _logger.LogErrorAsync(ex);
                        }

                    }
                }


                
            }
            catch (Exception ex)
            {
                _logger.LogErrorAsync(ex);
            }

            return runs;
        }
    }
}