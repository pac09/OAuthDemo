using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Models
{
    public class ReposModel
    {
        public string GitHubAvatar { get; set; }
        public string GitHubLogin { get; set; }
        public string GitHubName { get; set; }
        public string GitHubUrl { get; set; }

        public IReadOnlyList<Repository> Repositories { get; set; }
    }
}
