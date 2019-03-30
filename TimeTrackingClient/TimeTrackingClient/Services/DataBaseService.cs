using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using TimeTrackingClient.Constants;

namespace TimeTrackingClient.Services
{
    class DataBaseService
    {
        private static string _connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\TemporaryStorage.mdf;Integrated Security=True;";
        private static SqlConnection _sqlConnection;

        public DataBaseService()
        {
            _sqlConnection = new SqlConnection(_connectionString);
            _sqlConnection.Open();
        }

        public void AddStreamingDataToTemporaryStorage(StreamingData _streamingData)
        {
            string queryString = "INSERT INTO ActivityStaff (ApplicationAlias, ApplicationTitle, ApplicationImage, StaffAlias, ActivityTime)" +
                $" VALUES (N'{ _streamingData.ApplicationAlias}', N'{ _streamingData.ApplicationTitle}', N'{_streamingData.ApplicationImage}'," +
                $"N'{_streamingData.StaffAlias}', '{_streamingData.ActivityTime}')";


            SqlCommand command = new SqlCommand(queryString, _sqlConnection);
            command.ExecuteNonQuery();
        }

        public List<StreamingData> GetTemporaryStorageList()
        {
            List<StreamingData> streamingDataList = new List<StreamingData>();

            string queryString = "SELECT * FROM ActivityStaff ORDER BY ActivityTime ASC";

            SqlCommand command = new SqlCommand(queryString, _sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows) // если есть данные
            {
                while (reader.Read())
                {
                    var z = reader["ApplicationAlias"].ToString();
                    StreamingData streamingData = new StreamingData()
                    {
                        ApplicationAlias = reader["ApplicationAlias"].ToString(),
                        ApplicationTitle = reader["ApplicationTitle"].ToString(),
                        ApplicationImage = reader["ApplicationImage"].ToString(),
                        StaffAlias = reader["StaffAlias"].ToString(),
                        ActivityTime = Convert.ToInt32(reader["ActivityTime"])
                    };
                    streamingDataList.Add(streamingData);
                }
            }
            reader.Close();

            return streamingDataList;
        }


        public void DeleteTemporaryStorageByActivityTime(Int32 activityTime)
        {
            List<StreamingData> streamingDataList = new List<StreamingData>();

            string queryString = $"DELETE FROM ActivityStaff WHERE ActivityTime = {activityTime}";

            SqlCommand command = new SqlCommand(queryString, _sqlConnection);
            command.ExecuteNonQueryAsync();
        }
    }
}
