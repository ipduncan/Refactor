
namespace AimHealth.Safari.BulkPosting.Bus
{
    public class LineOfBusiness
    {
        private string _name;
        private string _abbreviation;

        public enum Abbreviation
        { CB, DM}

        public LineOfBusiness(Abbreviation abbreviation)
        {
            _abbreviation = abbreviation.ToString();
            _name = EvaluateAbbreviation(abbreviation.ToString());
        }

        public string Name
        {
            get { return _name; }
        }

        public string ShortName
        {
            get { return _abbreviation; }
            set { 
                    _abbreviation = value;
                    _name = EvaluateAbbreviation(_abbreviation);
                }
        }

        private string EvaluateAbbreviation(string abbreviation)
        {

            switch (abbreviation)
            {
                case "CB":
                    _name = "Credit Balance";
                    break;
                case "DM":
                    _name = "Data Mining";
                    break;
                default:
                    break;

            }

            return _name;
        }
    }

    
}
