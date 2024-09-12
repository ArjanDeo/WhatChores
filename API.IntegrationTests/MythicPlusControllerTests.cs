using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Models.BattleNet.Public.Character.MythicKeystone;
using Models.WhatChores.API;
using System.Collections.Generic;
using System.Net.Http.Json;

namespace API.IntegrationTests
{
    public class MythicPlusControllerTests
    {
        [Fact]
        public async Task CharacterRunsLibrary_ReturnsOkData()
        {
            var application = new WhatChoresWebApplicationFactory();
            var client = application.CreateClient();

            var response = await client.GetAsync("api/v1/mythicplus/CharacterRunsLibrary?name=furyshiftz&realm=tichondrius&region=us");

            response.EnsureSuccessStatusCode();

            var characterRunsLibraryResponse = await response.Content.ReadFromJsonAsync<WoWCharacterMythicKeystoneModel>();

            characterRunsLibraryResponse.character.Should().NotBeNull();
        }
        [Fact]
        public async Task GetWoWNews_Works()
        {
            WhatChoresWebApplicationFactory application = new();
            HttpClient client = application.CreateClient();

            HttpResponseMessage response = await client.GetAsync("api/v1/general/wowNews");
            response.EnsureSuccessStatusCode();

            List<NewsModel> getWoWNewsResponse = await response.Content.ReadFromJsonAsync<List<NewsModel>>();

            getWoWNewsResponse.Should().NotBeNull();
            getWoWNewsResponse.Should().AllBeOfType<NewsModel>();

            foreach(NewsModel post in getWoWNewsResponse)
            {
                post.Description.Should().NotBeNullOrWhiteSpace();
                post.Title.Should().NotBeNullOrWhiteSpace();
                post.Link.Should().NotBeNullOrWhiteSpace();
                post.Image.Should().NotBeNullOrWhiteSpace();
            }

        }
        [Fact]
        public async Task GetWoWNews_Works_WithLimit()
        {            
            WhatChoresWebApplicationFactory application = new();
            HttpClient client = application.CreateClient();

            int limit = 5;

            HttpResponseMessage response = await client.GetAsync($"api/v1/general/wowNews?limit={limit}");
            response.EnsureSuccessStatusCode();

            List<NewsModel> getWoWNewsResponse = await response.Content.ReadFromJsonAsync<List<NewsModel>>();

            getWoWNewsResponse.Should().NotBeNull();
            getWoWNewsResponse.Should().AllBeOfType<NewsModel>();
            getWoWNewsResponse.Count.Should().Be(limit);

            foreach (NewsModel post in getWoWNewsResponse)
            {
                post.Description.Should().NotBeNullOrWhiteSpace();
                post.Title.Should().NotBeNullOrWhiteSpace();
                post.Link.Should().NotBeNullOrWhiteSpace();
                post.Image.Should().NotBeNullOrWhiteSpace();
            }
        }
    }
}