using Ionic.Fun.MusicSearch.Model;
using System.Collections.Generic;


namespace Ionic.Fun.MusicSearch.Provider
{
    public interface IMusicProvider
    {
        string Name { get; }

        string getDownloadUrl(Song song);
        List<Song> SearchSongs(string keyword, int page, int pageSize);
    }
}