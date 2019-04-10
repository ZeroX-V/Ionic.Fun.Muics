using Ionic.Fun.MusicSearch.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Ionic.Fun.MusicSearch.Provider
{
    public class QQProvider : IMusicProvider
    {
        static HttpConfig DEFAULT_CONFIG = new HttpConfig
        {
            Referer = "http://m.y.qq.com",

        };

        public string Name { get; } = "QQ";

        static string[] prefixes = new string[] { "M800", "M500", "C400" };

        public List<Song> SearchSongs(string keyword, int page, int pageSize)
        {
            var searchResult = HttpHelper.GET(string.Format("http://c.y.qq.com/soso/fcgi-bin/search_for_qq_cp?w={0}&format=json&p={1}&n={2}", keyword, page, pageSize), DEFAULT_CONFIG);
            var searchResultJson = JArray.Parse(JObject.Parse(searchResult)["data"]["song"]["list"].ToString());
            //return json.mods.itemlist.data.collections[0];
            var result = new List<Song>();

            var index = 1;
            foreach (var songItem in searchResultJson)
            {
                var song = new Song
                {
                    id = songItem["songmid"].ToString(),
                    name = songItem["songname"].ToString(),
                    album = songItem["albumname"].ToString(),
                    rate = 128,
                    size = Convert.ToDouble(songItem["size128"]),
                    source = Name,
                    index = index++,
                    duration = Convert.ToDouble(songItem["interval"])
                };
                song.singer = "";
                foreach (var ar in songItem["singer"])
                {
                    song.singer += ar["name"] + " ";
                }
                result.Add(song);
            }

            return result;

        }

        public string getDownloadUrl(Song song)
        {
            var guid = Guid.NewGuid();
            var key = JObject.Parse(HttpHelper.GET(string.Format("http://base.music.qq.com/fcgi-bin/fcg_musicexpress.fcg?guid={0}&format=json&json=3", guid), DEFAULT_CONFIG))["key"];
            foreach (var prefix in prefixes)
            {

                var musicUrl = string.Format("http://dl.stream.qqmusic.qq.com/{0}{1}.mp3?vkey={2}&guid={3}&fromtag=1", prefix, song.id, key, guid);
                if (HttpHelper.GetUrlContentLength(musicUrl) > 0)
                {
                    return musicUrl;
                }
            }

            return null;

        }

    }
}
