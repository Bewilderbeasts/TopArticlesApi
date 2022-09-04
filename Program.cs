using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

public class ArticleContent
{
    public string? title { get; set; }
    public string? url { get; set; }
    public string author { get; set; }
    public int? num_comments { get; set; }
    public int? story_id { get; set; }
    public string? story_title { get; set; }
    public string? story_url { get; set; }
    public int? parent_id { get; set; }
    public int created_at { get; set; }

}

public class Article
{
    public int page { get; set; }
    public int per_page { get; set; }
    public int total { get; set; }
    public int total_pages { get; set; }
    public List<ArticleContent> data { get; set; }

}

class Result
{

    /*
     * Complete the 'topArticles' function below.
     *
     * The function is expected to return a STRING_ARRAY.
     * The function accepts following parameters:
     *  1. STRING username
     *  2. INTEGER limit
     *
     * base url for copy/paste
     * https://jsonmock.hackerrank.com/api/articles?author=<username>&page=<pageNumber>
     */

    public static async Task<List<string>> topArticles(string username, int limit)
    {
        List<string> articles = new List<string>();

        List<ArticleContent> articleContents = await getArticles(username, limit);
        List<ArticleContent> orderedByComments = articleContents.OrderByDescending(o => o.num_comments).ToList();

        ArticleContent firstarticleContent = orderedByComments.First();
        string topTitle;
        if (firstarticleContent.title != null)
        {
            topTitle = firstarticleContent.title;
            articles.Add(topTitle);
        }
        else if (firstarticleContent.title == null && firstarticleContent.story_title != null)
        {
            topTitle = firstarticleContent.story_title;
            articles.Add(topTitle);
        }

        return articles;
    }

    public static async Task<List<ArticleContent>> getArticles(string username, int limit)
    {
        var client = new HttpClient();
        var response = await client.GetAsync($"https://jsonmock.hackerrank.com/api/articles?author={username}&page={limit}");

        var jsonString = await response.Content.ReadAsStringAsync();

        Article article = JsonSerializer.Deserialize<Article>(jsonString);
        List<ArticleContent> articleContents = article.data;
        return articleContents;
    }

}

class Solution
{
    public static void Main(string[] args)
    {
        //TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);

        string username = Console.ReadLine();

        int limit = Convert.ToInt32(Console.ReadLine().Trim());

        Task.Run(async () =>
        {
            List<string> result =  await Result.topArticles(username, limit);
            Console.WriteLine(String.Join("\n", result));

        }).GetAwaiter().GetResult();

        //textWriter.Flush();
        //textWriter.Close();
    }
}
