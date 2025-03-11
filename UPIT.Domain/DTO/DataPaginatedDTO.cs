namespace UPIT.Domain.DTOs
{
    public class DataPaginatedDTO<T>
    {
        public int Total {  get; set; }
        public List<T> Data { get; set; }

        public DataPaginatedDTO(int total, List<T> data)
        {
            Total = total;
            Data = data;
        }
    }
}
