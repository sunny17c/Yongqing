using Microsoft.Data.SqlClient;
using System.Data;
using WebApplication1.Interfaces;
using Yongqing.Database;
using Yongqing.Databases.Test;

namespace WebApplication1.Repositorys;
public class BooksRepository : IBooksRepository
{
    private readonly YongqingContext _context;

    public BooksRepository(YongqingContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Books>> GetAllAsync()
    {
        string sql = @"
            select * from [Books]
        ";
        var result = await _context.GetResultAsync<Books>(sql);

        return result;
    }

    public async Task<Books?> GetByIdAsync(int id)
    {
        var parameters = new SqlParameter[]
        {
            new () { ParameterName = "@id", DbType = DbType.Int32, Value = id },
        };
        string sql = @"
            select * from [Books] where [id] = @id
        ";
        var result = (await _context.GetResultAsync<Books>(sql, parameters)).FirstOrDefault();

        return result;
    }

    public async Task<bool> CreateAsync(Books book)
    {
        var parameters = new SqlParameter[]
        {
            new () { ParameterName = "@Title", DbType = DbType.String, Value = book.Title },
            new () { ParameterName = "@Author", DbType = DbType.String, Value = book.Author  },
            new () { ParameterName = "@PublishDate", DbType = DbType.Date, Value = book.PublishDate.HasValue ? book.PublishDate : DBNull.Value },
            new () { ParameterName = "@Price", DbType = DbType.Decimal, Value = book.Price.HasValue ? book.Price : DBNull.Value },
        };
        string sql = @"
            insert into [Books] ([Title], [Author], [PublishDate], [Price]) values (@Title, @Author, @PublishDate, @Price)
        ";
        var result = await _context.ExecuteSqlAsync(sql, parameters);

        return result > 0 ? true : false;
    }

    public async Task<bool> UpdateAsync(Books book)
    {
        var parameters = new SqlParameter[]
        {
            new () { ParameterName = "@Id", DbType = DbType.Int32, Value = book.Id },
            new () { ParameterName = "@Title", DbType = DbType.String, Value = book.Title },
            new () { ParameterName = "@Author", DbType = DbType.String, Value = book.Author  },
            new () { ParameterName = "@PublishDate", DbType = DbType.Date, Value = book.PublishDate.HasValue ? book.PublishDate : DBNull.Value },
            new () { ParameterName = "@Price", DbType = DbType.Decimal, Value = book.Price.HasValue ? book.Price : DBNull.Value },
        };
        string sql = @"
            update [Books] set [Title] = @Title, [Author] = @Author, [PublishDate] = @PublishDate, [Price] = @Price where [Id] = @Id
        ";
        var result = await _context.ExecuteSqlAsync(sql, parameters);

        return result > 0 ? true : false;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var parameters = new SqlParameter[]
        {
            new () { ParameterName = "@Id", DbType = DbType.Int32, Value = id },
        };
        string sql = @"
            delete from [Books] where [Id] = @Id
        ";
        var result = await _context.ExecuteSqlAsync(sql, parameters);

        return result > 0 ? true : false;
    }
}
