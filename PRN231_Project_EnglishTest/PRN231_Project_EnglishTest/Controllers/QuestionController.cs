using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN231_Project_EnglishTest.DTO.DoExam;
using PRN231_Project_EnglishTest.Dto;
using PRN231_Project_EnglishTest.Models;
using Microsoft.EntityFrameworkCore;

namespace PRN231_Project_EnglishTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly Prn231Project1Context context;
        private IMapper mapper;
        public QuestionController(Prn231Project1Context context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public IActionResult GetQuestionByQuestionId(int id)
        {
            try
            {
                var question = context.Questions
                    .Include(o=> o.Options)
                    .Where(x => x.QuestionId == id)
                    .FirstOrDefault();
            
             

                return Ok(question);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public IActionResult AddListQuest(QuestionDto question)
        {
            try
            {
                var q = mapper.Map<Question>(question);
                context.Questions.Add(q);
                context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateQuestion(QuestionDto question, int id)
        {
            try
            {


                var temp = context.Questions.Where(q => q.QuestionId == id).FirstOrDefault();
                if (temp == null)
                    return NotFound();
                var q = mapper.Map<Question>(question);

                temp.QuestionText = q.QuestionText;
                temp.TestId = q.TestId;

                var listOptions = context.Options.ToList();
                context.Options.RemoveRange(listOptions);
           
                var resultdetail = context.ResultDetails.Where(rd => rd.QuestionId == temp.QuestionId).ToList();
                context.ResultDetails.RemoveRange(resultdetail);
                temp.Options = q.Options;
            
                context.Questions.Update(temp);
                context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteQuestion(int id)
        {
            try
            {
                var temp = context.Questions.Where(q => q.QuestionId == id).FirstOrDefault();

                if (temp == null)
                    return NotFound();

                var listOptions = context.Options.Where(o=> o.QuestionId==temp.QuestionId).ToList();
                context.Options.RemoveRange(listOptions);

                var resultdetail = context.ResultDetails.Where(rd => rd.QuestionId == temp.QuestionId).ToList();
                context.ResultDetails.RemoveRange(resultdetail);

                context.Questions.Remove(temp);
                context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
    }
}
