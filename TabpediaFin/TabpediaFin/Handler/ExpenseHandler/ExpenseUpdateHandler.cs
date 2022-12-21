﻿using TabpediaFin.Domain;
using TabpediaFin.Domain.Expense;
using TabpediaFin.Handler.Item;
using TabpediaFin.Handler.ItemItemCategoryHandler;
using TabpediaFin.Handler.ItemUnitMeasureHandler;

namespace TabpediaFin.Handler.ExpenseHandler;

public class ExpenseUpdateHandler : IRequestHandler<ExpenseUpdateDto, RowResponse<ExpenseFetchDto>>
{
    private readonly FinContext _context;
    private readonly ICurrentUser _currentUser;

    public ExpenseUpdateHandler(FinContext db, ICurrentUser currentUser)
    {
        _context = db;
        _currentUser = currentUser;
    }

    public async Task<RowResponse<ExpenseFetchDto>> Handle(ExpenseUpdateDto request, CancellationToken cancellationToken)
    {
        var result = new RowResponse<ExpenseFetchDto>();
        int expenseId;
        List<ExpenseTag> expenseTag = new List<ExpenseTag>();
        List<ExpenseAttachment> expenseAttachment = new List<ExpenseAttachment>();

        List<ExpenseFetchTag> expenseFetchTag = new List<ExpenseFetchTag>();
        List<ExpenseFetchAttachment> expenseFetchAttachment = new List<ExpenseFetchAttachment>();

        try
        {
            var expenses = await _context.Expense.FirstAsync(x => x.Id == request.Id && x.TenantId == _currentUser.TenantId, cancellationToken);
            expenses.TransNum = request.TransNum;
            expenses.TransDate = request.TransDate;
            expenses.ContactId = request.ContactId;
            expenses.PaymentMethodId = request.PaymentMethodId;
            expenses.PaymentTermId = request.PaymentTermId;
            expenses.Amount = request.Amount;
            expenses.DiscountTypeId = request.DiscountTypeId;
            expenses.DiscountPercent = request.DiscountPercent;
            expenses.DiscountAmount = request.DiscountAmount;
            expenses.Notes = request.Notes;
            expenses.Description = request.Description;
            expenses.TaxId = request.TaxId;

            expenseId = request.Id;
            List<int> idUpdateExpenseTag = new List<int>();
            List<int> idUpdateExpenseAttachment = new List<int>();


            if (request.ExpenseTagList.Count > 0)
            {
                foreach (ExpenseUpdateTag expTag in request.ExpenseTagList)
                {
                    idUpdateExpenseTag.Add(expTag.Id);
                    expenseTag.Add(new ExpenseTag
                    {
                        Id = expTag.Id,
                        TagId = expTag.TagId,
                        TransId = expenseId,
                        CreatedUid = _currentUser.UserId
                    });
                    expenseFetchTag.Add(new ExpenseFetchTag
                    {
                        Id = expTag.Id,
                        TagId = expTag.TagId,
                        TransId = expenseId
                    });
                }
                _context.ExpenseTag.UpdateRange(expenseTag);
            }

            if (request.AttachmentFile.Count > 0)
            {
                foreach (ExpenseAttachmentUpdate i in request.AttachmentFile)
                {
                    idUpdateExpenseAttachment.Add(i.Id);
                    expenseAttachment.Add(new ExpenseAttachment
                    {
                        Id = i.Id,
                        FileName = i.FileName,
                        FileUrl = i.FileUrl,
                        Extension = i.Extension,
                        FileSize = i.FileSize,
                        CreatedUid = _currentUser.UserId,
                        TransId = expenseId
                    });
                    expenseFetchAttachment.Add(new ExpenseFetchAttachment
                    {
                        Id = i.Id,
                        FileName = i.FileName,
                        FileUrl = i.FileUrl,
                        Extension = i.Extension,
                        FileSize = i.FileSize,
                        TransId = expenseId
                    });
                }
                _context.ExpenseAttachment.UpdateRange(expenseAttachment);
            }

            List<ExpenseTag> expenseTagList = _context.ExpenseTag.Where<ExpenseTag>(x => x.TransId == request.Id && x.TenantId == _currentUser.TenantId && !idUpdateExpenseTag.Contains(x.Id)).ToList();
            List<ExpenseAttachment> expenseAttachmentList = _context.ExpenseAttachment.Where<ExpenseAttachment>(x => x.TransId == request.Id && x.TenantId == _currentUser.TenantId && !idUpdateExpenseAttachment.Contains(x.Id)).ToList();
            _context.ExpenseTag.RemoveRange(expenseTagList);
            _context.ExpenseAttachment.RemoveRange(expenseAttachmentList);
            await _context.SaveChangesAsync(cancellationToken);

            var row = new ExpenseFetchDto()
            {
                Id = request.Id,
                TransNum = request.TransNum,
                TransDate = request.TransDate,
                ContactId = request.ContactId,
                PaymentMethodId = request.PaymentMethodId,
                PaymentTermId = request.PaymentTermId,
                Amount = request.Amount,
                DiscountTypeId = request.DiscountTypeId,
                DiscountPercent = request.DiscountPercent,
                DiscountAmount = request.DiscountAmount,
                Notes = request.Notes,
                Description = request.Description,
                TaxId = request.TaxId,
                ExpenseTagList = expenseFetchTag,
                ExpenseAttachmentList = expenseFetchAttachment,
            };

            result.IsOk = true;
            result.ErrorMessage = string.Empty;
            result.Row = row;
        }
        catch (Exception ex)
        {
            result.IsOk = false;
            result.ErrorMessage = ex.Message;
        }
        return result;
    }

}



public class ExpenseUpdateDto : IRequest<RowResponse<ExpenseFetchDto>>
{
    public int Id { get; set; }
    public string TransNum { get; set; } = string.Empty;
    public DateTime? TransDate { get; set; }
    public int ContactId { get; set; } = 0;
    public int PaymentMethodId { get; set; } = 0;
    public int PaymentTermId { get; set; } = 0;
    public int Amount { get; set; } = 0;
    public int DiscountTypeId { get; set; } = 0;
    public int DiscountPercent { get; set; } = 0;
    public int DiscountAmount { get; set; } = 0;
    public string Notes { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int TaxId { get; set; } = 0;
    public List<ExpenseAttachmentUpdate> AttachmentFile { get; set; }
    public List<ExpenseUpdateTag> ExpenseTagList { get; set; }
}

public class ExpenseAttachmentUpdate
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string FileSize { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public int TransId { get; set; }
}
public class ExpenseUpdateTag
{
    public int Id { get; set; }
    public int TagId { get; set; }
}