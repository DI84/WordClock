

namespace Wordclock
{
    public class BaseModel
    {
        private string Texts;

        public int Row { get; set; }
        public int Column { get; set; }
        public double Opacity { get; set; }
        public string Text { get {return Texts; } set {Texts = value;} }
    }
}
