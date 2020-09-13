using BadMelon.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BadMelon.Tests
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestDataController : ControllerBase
    {
        private readonly BadMelonDataContext _db;

        public TestDataController(BadMelonDataContext db)
        {
            _db = db;
        }

        [HttpGet("verification-codes/{id}")]
        public async Task<Guid> GetVerificationCodeByUsername(string id)
        {
            return (await _db.Users.SingleOrDefaultAsync(u => u.NormalizedUserName == id.ToUpper()))?.EmailVerificationCode ?? Guid.Empty;
        }
    }
}