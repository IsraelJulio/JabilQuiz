using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JabilQuiz.Model
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int GameId { get; set; }
    }
}
