using BudgetPlanner.DomainLayer.Enums;
using BudgetPlanner.DomainLayer.Models;

namespace BudgetPlanner.DomainLayer.Services
{
    public class PrognosisService
    {
        private readonly BudgetPostService _postService;

        public PrognosisService(BudgetPostService postService)
        {
            _postService = postService;
        }


        #region == PROGNOSIS VIEWMODEL ===========================

        public List<Prognosis> GeneratePrognosisRange(DateTime centerDate, int previousMonths = 5, int nextMonths = 4)
        {
            var allPosts = _postService.GetAllPosts();

            var list = new List<Prognosis>();

            int startOffset = -previousMonths;
            int endOffset = nextMonths;

            for (int offset = startOffset; offset <= endOffset; offset++)
            {
                var target = centerDate.AddMonths(offset);
                var from = new DateTime(target.Year, target.Month, 1);
                var to = from.AddMonths(1).AddDays(-1);

                var p = new Prognosis
                {
                    Id = offset + previousMonths + 1,
                    Month = from.ToString("MMMM yyyy"),
                    FromDate = from,
                    ToDate = to
                };

                CalculatePrognosisForMonth(p, allPosts);

                list.Add(p);
            }

            return list;
        }


        // Beräknar intäkter/utgifter för en månad.
        private void CalculatePrognosisForMonth(Prognosis prognosis, List<BudgetPost> allPosts)
        {
            prognosis.BudgetPosts.Clear();
            prognosis.MonthlyIncome = 0;
            prognosis.MonthlyExpense = 0;

            var posts = allPosts.Where(p =>
                // Engångsposter inom den aktuella månaden
                (p.Recurring == Recurring.None &&
                 p.Date.HasValue &&
                 p.Date.Value >= prognosis.FromDate &&
                 p.Date.Value <= prognosis.ToDate &&
                 p.Date.Value <= DateTime.Today)

                // Recurring-poster alltid med
                || p.Recurring != Recurring.None
            );

            foreach (var post in posts)
            {
                var monthlyValue = ConvertToMonthlyValue(post);

                if (post.PostType == BudgetPostType.Income)
                    prognosis.MonthlyIncome += monthlyValue;
                else
                    prognosis.MonthlyExpense += Math.Abs(monthlyValue);

               prognosis.BudgetPosts.Add(post);
            }

            prognosis.TotalSum = prognosis.MonthlyIncome - prognosis.MonthlyExpense;
        }

        // Kalkylerar belopp.baserat på återkommandetyp (värde)
        private decimal ConvertToMonthlyValue(BudgetPost post)
        {
            var amount = (decimal)post.Amount;

            return post.Recurring switch
            {
                Recurring.Daily => amount * 30,
                Recurring.Weekly => amount * 4.33m,
                Recurring.Monthly => amount,
                Recurring.Yearly => amount / 12,
                _ => amount
            };
        }

        #endregion
    }
}
