namespace ContainersApiTask.Models
{
    public class Query
    {
        public Query()
        {
            search = "";
            sortBy = "";
        }
        public string search { get; set; }
        public string sortBy { get; set; }

        public override string ToString()
        {
            return "search: " + search + " sortby: " + sortBy ;
        }
    }
}
