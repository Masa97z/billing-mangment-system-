using billing_mangment_system.Models;
using billing_mangment_system_v2.Dtos;
using billing_mangment_system_v2.ICollectionService;
using billing_mangment_system_v2.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

[Route("api/[controller]")]
[ApiController]
public class UnitController : ControllerBase
{
    private readonly ICollectionBillService _collectionBillService;
    private readonly IUserService _userService;
    private readonly IAmount _amount;
    int inv;
    public UnitController(ICollectionBillService collectionBillService, IUserService userService , IAmount amount)
    {
        _collectionBillService = collectionBillService;
        _userService = userService;
        _amount = amount;
    }

    // GET: api/CollectionBill
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Bills>>> Get()
    {
        var bills = await _collectionBillService.GetBillsAsync();
        return Ok(bills);
    }

    // GET: api/CollectionBill/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Bills>> Get(string id)
    {
        var bill = await _collectionBillService.GetBillByIdAsync(id);
        if (bill == null)
        {
            return NotFound();
        }
        return Ok(bill);
    }

    // POST: api/CollectionBill
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] BillsDto bill)
    {
        try
        {
            // Retrieve necessary data from services
            var bills = await _collectionBillService.GetBillsAsync();
            var users = await _userService.GetUserAsync();
            var currentInvoice = (await _amount.GetInvocesAsync()).FirstOrDefault(u => u.CostumerId == bill.Userid);

            if (users == null || !users.Any())
                return BadRequest("No users found.");

            var currentUser = users.FirstOrDefault(u => u.CostumerId == bill.Userid);
            // Assuming the first user is the one we need
            var sortedBills = bills.OrderByDescending(b => b.PostTime).ToList();
            var lastBill = sortedBills.FirstOrDefault( u =>u.CostumerId == bill.Userid);

            int invoiceAmount = 0;
            var newBill = new Bills
            {
                CostumerId = bill.Userid,
                PostUnit = bill.PostUnit,
                PreUnit = lastBill?.PostUnit ?? 0,
                PostTime = DateTime.Now,
                PreTime = lastBill?.PostTime ?? DateTime.Now
            };
             var usage = bill.PostUnit - lastBill.PostUnit;
            await _collectionBillService.CreateBillAsync(newBill);
            // Calculate the invoice amount
            
               

                switch (currentUser.typeAccount)
                {
                    case "1":
                        invoiceAmount = CalculateInvoiceAmountTier1(usage);
                        break;
                    case "2":
                        invoiceAmount = usage * 120;
                        break;
                    case "3":
                        invoiceAmount = CalculateInvoiceAmountTier3(usage);
                        break;
                    case "4":
                        invoiceAmount = usage * 60;
                        break;
                   
                }

                // Prepare invoice update
                var updatedInvoice = new Invoces
                {
                    CostumerId = currentUser.CostumerId,
                    Id = currentInvoice.Id,
                    Deps = currentInvoice.TotalAmount,
                    CurrentAmount = invoiceAmount,
                    TotalAmount = currentInvoice.TotalAmount + invoiceAmount
                };

                // Update invoice in the database
                await _amount.UpdateInvocesAsync(updatedInvoice.CostumerId, updatedInvoice);
            

          
            return CreatedAtAction(nameof(Get), newBill);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while processing the request: {ex.Message}");
        }
    
    }

    private static int CalculateInvoiceAmountTier1(int usage)
    {
        if (usage < 1500)
            return usage * 10;
        else if (usage < 3000)
            return usage * 35;
        else if (usage < 4000)
            return usage * 80;
        else
            return usage * 120;
    }

    private static int CalculateInvoiceAmountTier3(int usage)
    {
        if (usage < 1000)
            return usage * 120;
        else if (usage < 2000)
            return usage * 80;
        else
            return usage * 120;
    }


    [HttpPost("new")]
    public async Task<IActionResult> Post([FromBody] Bills bill)
    {


        await _collectionBillService.CreateBillAsync(bill);
        return CreatedAtAction(nameof(Get), bill);

    }
        // PUT: api/CollectionBill/5
        [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] Bills bill)
    {
        if (id != bill.Id)
        {
            return BadRequest();
        }

        try
        {
            await _collectionBillService.UpdateBillAsync(id, bill);
        }
        catch (Exception)
        {
            return NotFound();
        }

        return NoContent();
    }

    // DELETE: api/CollectionBill/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var existingBill = await _collectionBillService.GetBillByIdAsync(id);
        if (existingBill == null)
        {
            return NotFound();
        }

        await _collectionBillService.DeleteBillAsync(id);
        return NoContent();
    }


    
}
