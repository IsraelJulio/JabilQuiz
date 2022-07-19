using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JabilQuiz.Model
{
    public class Alternative
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool RightAnswer { get; set; }
        public int QuizId { get; set; }
    }
}
