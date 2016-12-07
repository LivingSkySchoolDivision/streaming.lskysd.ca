using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSKYStreamingCore
{
    public class LiveBroadcastRepository
    {
        private const int liveBroadcastIDLength = 5;
        private readonly Dictionary<string, LiveBroadcast> _cache;

        private LiveBroadcast dbDataReaderToLiveBroadcast(SqlDataReader dataReader)
        {
            return new LiveBroadcast()
            {
                ID = dataReader["id"].ToString().Trim(),
                Name = dataReader["name"].ToString().Trim(),
                Description = dataReader["description"].ToString().Trim(),
                Location = dataReader["location"].ToString().Trim(),
                ThumbnailURL = dataReader["thumbnail_url"].ToString().Trim(),
                Width = Parsers.ParseInt(dataReader["Width"].ToString().Trim()),
                Height = Parsers.ParseInt(dataReader["Height"].ToString().Trim()),
                StartTime = Parsers.ParseDate(dataReader["stream_start"].ToString().Trim()),
                EndTime = Parsers.ParseDate(dataReader["stream_end"].ToString().Trim()),
                IsHidden = Parsers.ParseBool(dataReader["hidden"].ToString().Trim()),
                IsPrivate = Parsers.ParseBool(dataReader["private"].ToString().Trim()),
                ForcedLive = Parsers.ParseBool(dataReader["force_online"].ToString().Trim()),
                YouTubeID = dataReader["youtube_id"].ToString().Trim(),
                IsDelayed = Parsers.ParseBool(dataReader["delayed"].ToString().Trim()),
                IsCancelled = Parsers.ParseBool(dataReader["cancelled"].ToString().Trim())
            };
        }

        public LiveBroadcastRepository()
        {
            _cache = new Dictionary<string, LiveBroadcast>();
            using (SqlConnection connection = new SqlConnection(GlobalStreamingSettings.dbConnectionString_ReadOnly))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = "SELECT * FROM live_streams ORDER BY stream_start DESC, stream_end DESC;";
                    sqlCommand.Connection.Open();
                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();

                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            LiveBroadcast parsedBroadcast = dbDataReaderToLiveBroadcast(dbDataReader);
                            _cache.Add(parsedBroadcast.ID, parsedBroadcast);
                        }
                    }

                    sqlCommand.Connection.Close();
                }
            }
        }

        public LiveBroadcast Get(string id)
        {
            return _cache.ContainsKey(id) ? _cache[id] : null;
        }

        public List<LiveBroadcast> GetAll()
        {
            return GetAll(true);
        }

        public List<LiveBroadcast> GetAll(bool includeHiddenBroadcasts)
        {
            if (includeHiddenBroadcasts)
            {
                return _cache.Values.ToList();
            }
            else
            {
                return _cache.Values.Where(lb => !lb.IsHidden).ToList();
            }
        }

        public List<LiveBroadcast> GetLive()
        {
            return _cache.Values.Where(lb => lb.IsLive && !lb.IsHidden).ToList();
        }

        public List<LiveBroadcast> GetUpcoming()
        {
            // Use end time, so that streams are still listed while they are happening
            return _cache.Values.Where(lb => DateTime.Now < lb.EndTime && !lb.IsHidden).ToList();
        }


        public void Insert(LiveBroadcast broadcast)
        {
            if (string.IsNullOrEmpty(broadcast.ID))
            {
                broadcast.ID = CreateNewID();
            }

            using (SqlConnection connection = new SqlConnection(GlobalStreamingSettings.dbConnectionString_ReadOnly))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = "INSERT INTO live_streams(id,name,location,description,thumbnail_url,width,height,stream_start,stream_end,hidden,private,force_online,youtube_id,delayed,cancelled)"
                                            + "VALUES(@ID,@NAME,@LOC,@DESC,@THUMB,@WIDTH,@HEIGHT,@STARTS,@ENDS,@ISHIDDEN,@ISPRIVATE,@FORCEONLINE,@YOUTUBEID,@DELAYED,@CANCELLED)";
                    sqlCommand.Parameters.AddWithValue("ID", broadcast.ID);
                    sqlCommand.Parameters.AddWithValue("NAME", broadcast.Name);
                    sqlCommand.Parameters.AddWithValue("DESC", broadcast.Description);
                    sqlCommand.Parameters.AddWithValue("LOC", broadcast.Location);
                    sqlCommand.Parameters.AddWithValue("THUMB", broadcast.ThumbnailURL);
                    sqlCommand.Parameters.AddWithValue("WIDTH", broadcast.Width);
                    sqlCommand.Parameters.AddWithValue("HEIGHT", broadcast.Height);
                    sqlCommand.Parameters.AddWithValue("STARTS", broadcast.StartTime);
                    sqlCommand.Parameters.AddWithValue("ENDS", broadcast.EndTime);
                    sqlCommand.Parameters.AddWithValue("ISHIDDEN", broadcast.IsHidden);
                    sqlCommand.Parameters.AddWithValue("ISPRIVATE", broadcast.IsPrivate);
                    sqlCommand.Parameters.AddWithValue("FORCEONLINE", broadcast.ForcedLive);
                    sqlCommand.Parameters.AddWithValue("YOUTUBEID", broadcast.YouTubeID);
                    sqlCommand.Parameters.AddWithValue("DELAYED", broadcast.IsDelayed);
                    sqlCommand.Parameters.AddWithValue("CANCELLED", broadcast.IsCancelled);
                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Connection.Close();
                }
            }
        }

        public void Update(LiveBroadcast broadcast)
        {
            using (SqlConnection connection = new SqlConnection(GlobalStreamingSettings.dbConnectionString_ReadOnly))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = "UPDATE live_streams SET name=@NAME, location=@LOC, description=@DESC, thumbnail_url=@THUMB, width=@WIDTH, height=@HEIGHT,"
                                                + " stream_start=@STARTS, stream_end=@ENDS, hidden=@ISHIDDEN, private=@ISPRIVATE, force_online=@FORCEONLINE, youtube_id=@YOUTUBEID, cancelled=@CANCELLED, delayed=@DELAYED"
                                                + " WHERE id=@ID";
                    sqlCommand.Parameters.AddWithValue("ID", broadcast.ID);
                    sqlCommand.Parameters.AddWithValue("NAME", broadcast.Name);
                    sqlCommand.Parameters.AddWithValue("LOC", broadcast.Location);
                    sqlCommand.Parameters.AddWithValue("DESC", broadcast.Description);
                    sqlCommand.Parameters.AddWithValue("THUMB", broadcast.ThumbnailURL);
                    sqlCommand.Parameters.AddWithValue("WIDTH", broadcast.Width);
                    sqlCommand.Parameters.AddWithValue("HEIGHT", broadcast.Height);
                    sqlCommand.Parameters.AddWithValue("STARTS", broadcast.StartTime);
                    sqlCommand.Parameters.AddWithValue("ENDS", broadcast.EndTime);
                    sqlCommand.Parameters.AddWithValue("ISHIDDEN", broadcast.IsHidden);
                    sqlCommand.Parameters.AddWithValue("ISPRIVATE", broadcast.IsPrivate);
                    sqlCommand.Parameters.AddWithValue("FORCEONLINE", broadcast.ForcedLive);
                    sqlCommand.Parameters.AddWithValue("YOUTUBEID", broadcast.YouTubeID);
                    sqlCommand.Parameters.AddWithValue("DELAYED", broadcast.IsDelayed);
                    sqlCommand.Parameters.AddWithValue("CANCELLED", broadcast.IsCancelled);
                    sqlCommand.Connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Connection.Close();
                }
            }
        }

        public string CreateNewID()
        {
            string returnMe = string.Empty;
            do
            {
                returnMe = Crypto.GenerateID(liveBroadcastIDLength);
            } while ((returnMe != string.Empty) && (!IsLiveBroadcastIDAvailable(returnMe)));
            return returnMe;
        }

        private bool IsLiveBroadcastIDAvailable(string streamID)
        {
            if (string.IsNullOrEmpty(streamID.Trim()))
            {
                return false;
            }
            
            bool foundGivenID = false;

            using (SqlConnection connection = new SqlConnection(GlobalStreamingSettings.dbConnectionString_ReadOnly))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = "SELECT id FROM live_streams WHERE (id=@STREAMID)";
                    sqlCommand.Parameters.AddWithValue("STREAMID", streamID);
                    sqlCommand.Connection.Open();
                    SqlDataReader dbDataReader = sqlCommand.ExecuteReader();
                    if (dbDataReader.HasRows)
                    {
                        while (dbDataReader.Read())
                        {
                            foundGivenID = true;
                        }
                    }

                    sqlCommand.Connection.Close();
                }
            }

            return !foundGivenID;
        }


    }
}
