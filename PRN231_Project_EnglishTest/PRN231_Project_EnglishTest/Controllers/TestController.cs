﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN231_Project_EnglishTest.Dto;
using PRN231_Project_EnglishTest.Models;

namespace PRN231_Project_EnglishTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly Prn231Project1Context context;
        private IMapper mapper;
        public TestController(Prn231Project1Context context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllTest()
        {
            try
            {
                return Ok(context.Tests.ToList());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetTest(int id)
        {
            try
            {
                var test = context.Tests.FirstOrDefault(x => x.TestId == id);
                if (test == null)
                {
                    return NotFound();
                }
                return Ok(test);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTest(int id, TestDto testDto)
        {
            try
            {
                var temp =  context.Tests.Where(t => t.TestId == id).FirstOrDefault();
                if (temp == null)
                    return NotFound();

                Test test = mapper.Map<Test>(testDto);
                temp.TestName = test.TestName;
                temp.TestDescription = test.TestDescription;
                temp.Duration = test.Duration;
                context.Tests.Update(temp);
                context.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult AddTest(TestDto testDto)
        {
            try
            {
                Test test = mapper.Map<Test>(testDto);
                context.Tests.Add(test);
                context.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTest(int id)
        {
            try
            {
                var temp = context.Tests.Where(t => t.TestId == id).FirstOrDefault();
                if (temp == null)
                    return NotFound();
                context.Tests.Remove(temp);
                context.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
