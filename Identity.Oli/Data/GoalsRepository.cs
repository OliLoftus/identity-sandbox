using System.Text.RegularExpressions;
using Identity.Oli.Models;
using Identity.Oli.Models.Requests;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Identity.Oli.Data;

// Handles all database operations for the Goals collection.
public class GoalsRepository(MongoDbContext context) : IGoalsRepository
{
        private readonly IMongoCollection<GoalModel> _goals = context.Goals; // MongoDB collection for goals
        
        public async Task<List<GoalModel>> GetAllAsync()
        {
                return await _goals.Find(_ => true).ToListAsync();
        }

        public async Task<List<GoalModel>> GetAllAsync(GoalQuery query)
        {
                var result = await GetAllInternalAsync(query, includeCount: false);
                return result.Items;
        }

        public Task<(List<GoalModel> Items, long TotalCount)> GetAllWithCountAsync(GoalQuery query)
        {
                return GetAllInternalAsync(query, includeCount: true);
        }

        private async Task<(List<GoalModel> Items, long TotalCount)> GetAllInternalAsync(
                GoalQuery query,
                bool includeCount)
        {
                ArgumentNullException.ThrowIfNull(query);

                var filters = new List<FilterDefinition<GoalModel>>();

                if (!string.IsNullOrWhiteSpace(query.Category))
                {
                        var category = Regex.Escape(query.Category.Trim());
                        var regex = new BsonRegularExpression($"^{category}$", "i");
                        filters.Add(Builders<GoalModel>.Filter.Regex(goal => goal.Category, regex));
                }

                if (query.IsCompleted.HasValue)
                {
                        filters.Add(Builders<GoalModel>.Filter.Eq(goal => goal.IsCompleted, query.IsCompleted.Value));
                }

                if (query.DueFrom.HasValue)
                {
                        filters.Add(Builders<GoalModel>.Filter.Gte(goal => goal.DueDate, query.DueFrom.Value));
                }

                if (query.DueTo.HasValue)
                {
                        filters.Add(Builders<GoalModel>.Filter.Lte(goal => goal.DueDate, query.DueTo.Value));
                }

                if (!string.IsNullOrWhiteSpace(query.TitleContains))
                {
                        var contains = Regex.Escape(query.TitleContains.Trim());
                        var regex = new BsonRegularExpression(contains, "i");
                        filters.Add(Builders<GoalModel>.Filter.Regex(goal => goal.Title, regex));
                }

                var filter = filters.Count == 0
                        ? Builders<GoalModel>.Filter.Empty
                        : Builders<GoalModel>.Filter.And(filters);

                var find = _goals.Find(filter);

                var sort = BuildSort(query);
                if (sort != null)
                {
                        find = find.Sort(sort);
                }

                if (query.Page.HasValue || query.PageSize.HasValue)
                {
                        var page = query.Page ?? 1;
                        var pageSize = query.PageSize ?? 50;
                        var skip = (page - 1) * pageSize;
                        find = find.Skip(skip).Limit(pageSize);
                }

                var itemsTask = find.ToListAsync();
                var countTask = includeCount ? _goals.CountDocumentsAsync(filter) : Task.FromResult(0L);

                await Task.WhenAll(itemsTask, countTask);
                return (itemsTask.Result, countTask.Result);
        }

        private static SortDefinition<GoalModel>? BuildSort(GoalQuery query)
        {
                if (string.IsNullOrWhiteSpace(query.SortBy))
                {
                        return null;
                }

                var sortBy = query.SortBy.Trim().ToLowerInvariant();
                var descending = query.SortDir?.Trim().Equals("desc", StringComparison.OrdinalIgnoreCase) ?? false;

                return sortBy switch
                {
                        "title" => descending
                                ? Builders<GoalModel>.Sort.Descending(goal => goal.Title)
                                : Builders<GoalModel>.Sort.Ascending(goal => goal.Title),
                        "category" => descending
                                ? Builders<GoalModel>.Sort.Descending(goal => goal.Category)
                                : Builders<GoalModel>.Sort.Ascending(goal => goal.Category),
                        "duedate" => descending
                                ? Builders<GoalModel>.Sort.Descending(goal => goal.DueDate)
                                : Builders<GoalModel>.Sort.Ascending(goal => goal.DueDate),
                        "iscompleted" => descending
                                ? Builders<GoalModel>.Sort.Descending(goal => goal.IsCompleted)
                                : Builders<GoalModel>.Sort.Ascending(goal => goal.IsCompleted),
                        _ => null
                };
        }
        
        public async Task<GoalModel> GetByIdAsync(Guid id)
        {
                return await _goals.Find(goal => goal.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(GoalModel goal)
        {
                await _goals.InsertOneAsync(goal);
        }

        public async Task UpdateAsync(Guid id, GoalModel updatedGoal)
        {
                var update = Builders<GoalModel>.Update
                        .Set(goal => goal.Title, updatedGoal.Title)
                        .Set(goal => goal.Description, updatedGoal.Description)
                        .Set(goal => goal.Category, updatedGoal.Category)
                        .Set(goal => goal.DueDate, updatedGoal.DueDate)
                        .Set(goal => goal.IsCompleted, updatedGoal.IsCompleted)
                        .Set(goal => goal.Progress, updatedGoal.Progress);

                await _goals.UpdateOneAsync(goal => goal.Id == id, update);
        }

        public async Task DeleteAsync(Guid id)
        {
                await _goals.DeleteOneAsync(goal => goal.Id == id);
        }
}
