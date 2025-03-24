using API_TEST.Data;
using API_TEST.Helpers;
using API_TEST.Models;
using API_TEST.Repositories;
using AutoMapper;

namespace API_TEST.Services
{
    public interface IMediaServices
    {
        Task<string?> CreateMedia(MediaRequest request);
        Task<string?> UpdateMedia(int id, MediaRequest request);
        string? DeleteMedia(int id);

        MediaRespone GetMedia(int id);
        List<MediaRespone> GetListMedia();
    }
    public class MediaServices : IMediaServices
    {
        private readonly IRepositoryWrapper repositoryWrapper;
        private readonly IMapper mapper;

        public MediaServices(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            this.repositoryWrapper = repositoryWrapper;
            this.mapper = mapper;
        }

        public async Task<string?> CreateMedia(MediaRequest request)
        {
            Media media = mapper.Map<Media>(request);

            //Xử lý lưu file ảnh và video
            string uploadDirectory = Path.Combine("wwwroot");

            string file = await FileHelper.SaveFileAsync(request.FilePath, uploadDirectory);

            if (file != null)
            {
                media.FilePath = file;
            }

            repositoryWrapper.Media.Create(media);
            repositoryWrapper.Save();
            return null;

        }

        public async Task<string?> UpdateMedia(int id, MediaRequest request)
        {
            Media? media = repositoryWrapper.Media.FindByCondition(x => x.MediaId == id).FirstOrDefault();
            if (media == null)
            {
                return "Media not found";
            }

            mapper.Map(request, media);

            string uploadDirectory = Path.Combine("wwwroot");

            string file = await FileHelper.SaveFileAsync(request.FilePath, uploadDirectory);

            if (file != null)
            {
                media.FilePath = file;
            }

            repositoryWrapper.Media.Update(media);
            repositoryWrapper.Save();
            return null;
        }

        public string? DeleteMedia(int id)
        {
            Media? media = repositoryWrapper.Media.FindByCondition(x => x.MediaId == id).FirstOrDefault();
            if (media == null)
            {
                return "Media not found";
            }

            repositoryWrapper.Media.Delete(media);
            repositoryWrapper.Save();
            return null;
        }

        public MediaRespone GetMedia(int id)
        {
            Media? media = repositoryWrapper.Media.FindByCondition(x => x.MediaId == id).FirstOrDefault();
            if (media == null)
            {
                return null;
            }

            return mapper.Map<MediaRespone>(media);
        }

        public List<MediaRespone> GetListMedia()
        {
            List<Media> media = repositoryWrapper.Media.FindAll().ToList();
            return mapper.Map<List<MediaRespone>>(media);
        }
    }
}
