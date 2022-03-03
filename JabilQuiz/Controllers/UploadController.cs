using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using JabilQuiz.Model;
using Newtonsoft.Json;
using Interface.DataServices;

namespace WebApplication.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        readonly IQuizService _quizService;

        public UploadController(IQuizService quizService)
        {
            this._quizService = quizService;
        }
        [HttpPost("UploadFiles")]
        public async Task<IActionResult> UploadFiles(IFormFile file)
        {
            try
            {
                ISheet sheet;
                string fileName = Path.GetFileName(file.FileName);
                var fileExt = Path.GetExtension(fileName);

                if (fileExt == ".xls")
                {
                    HSSFWorkbook hssfwb = new HSSFWorkbook(file.OpenReadStream());
                    sheet = hssfwb.GetSheetAt(0);
                }
                else
                {
                    XSSFWorkbook hssfwb = new XSSFWorkbook(file.OpenReadStream());
                    sheet = hssfwb.GetSheetAt(0);
                }
                //throw new Exception($"METHOD NOT ALLOWED");             

                try
                {
                    List<Quiz> QuizList = new List<Quiz>();
                    var actualRow = sheet.GetRow(0);
                    bool skip = false;
                    
                    for (int row = 0; (row <= sheet.LastRowNum) && (skip == false); row++)
                    {
                        actualRow = sheet.GetRow(row);
                        if (actualRow.GetCell(0).StringCellValue == "")
                            skip = true;
                        else
                        {
                            Quiz quiz = new Quiz();                            
                            quiz.Question = actualRow.GetCell(0).StringCellValue;
                            quiz.Answer = actualRow.GetCell(1).StringCellValue;
                            QuizList.Add(quiz);
                        }
                    }
                    await this._quizService.SaveRangeAsync(QuizList);                 
                    return Ok(QuizList);

                }
                catch (Exception e) when (e.Message.Contains("METHOD NOT ALLOWED"))
                {
                    return StatusCode(StatusCodes.Status405MethodNotAllowed, e.Message);
                }
                catch (Exception e) when (e.Message.Contains("Not Found"))
                {
                    return StatusCode(StatusCodes.Status404NotFound, e.Message);
                }
                catch (Exception e)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
                }

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
            }

        }
    }
}