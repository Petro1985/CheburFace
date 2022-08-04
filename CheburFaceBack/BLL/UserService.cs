using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Mime;
using AutoMapper;
using SkillFactoryModule19.BLL.Models;
using SkillFactoryModule19.BLL.Validators;
using SkillFactoryModule19.DAL.Entities;
using SkillFactoryModule19.DAL.Repositories;
using SkillFactoryModule19.DAL.Repositories.Users;
using SkillFactoryModule19.Util;
    
namespace SkillFactoryModule19.BLL;

public class UserService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IValidator<User> _validator;
    private readonly IPhotoRepository _photoRepository;

    public UserService(IUserRepository userRepository, IValidator<User> validator, IMapper mapper, IPhotoRepository photoRepository)
    {
        _userRepository = userRepository;
        _validator = validator;
        _mapper = mapper;
        _photoRepository = photoRepository;
    }

    public async Task<OperationResult<Unit, string>> SetPhoto(User user, Stream photoStream)
    {
        var result = await _photoRepository.SavePhoto(user.Id, photoStream);
        if (!result.IsSuccessful)
        {
            return new OperationResult<Unit, string>(result.Error!);
        }
        
        user.Photo = result.Result; 
        await _userRepository.Update(_mapper.Map<UserEntity>(user));

        return new OperationResult<Unit, string>(Unit.Instance);
    }
        
    public async Task<OperationResult<Stream, string>> GetPhoto(User user)
    {
        var result = await _photoRepository.LoadPhoto(user.Id);
        if (!result.IsSuccessful)
        {
            return new OperationResult<Stream, string>(result.Error!);
        }
        return new OperationResult<Stream, string>(result.Result!); 
    }
        
    public async Task<OperationResult<Unit, IReadOnlyCollection<string>>> AddUser(User user)
    {
        var result = _validator.Validate(user);
        var userEntity = _mapper.Map<UserEntity>(user);
        
        if (result.IsError)
        {
            return new OperationResult<Unit, IReadOnlyCollection<string>>(result.GetErrors());
        }

        var isUnique = (await _userRepository.FindByEmail(userEntity.EMail)) is null;
        if (!isUnique)
        {
            return new OperationResult<Unit, IReadOnlyCollection<string>>(new List<string> {"User with this EMail is already registered"});
        } 
        
        await _userRepository.Create(userEntity);
        
        return new OperationResult<Unit, IReadOnlyCollection<string>>(Unit.Instance);
    }

    public async Task<User?> GetUser(int id)
    {
        var userEntity = await _userRepository.FindById(id);
        return _mapper.Map<User>(userEntity);
    }
    public async Task<User?> GetUser(string email)
    {
        var userEntity = await _userRepository.FindByEmail(email);
        return _mapper.Map<User>(userEntity);
    }

    public async Task<OperationResult<Unit, IReadOnlyCollection<string>>> UpdateUser(User user)
    {
        var result = _validator.Validate(user);
        if (result.IsError)
        {
            return new OperationResult<Unit, IReadOnlyCollection<string>>(result.GetErrors());
        }
        
        var entityInDb = (await _userRepository.FindByEmail(user.EMail));
        
        if (entityInDb is not null && entityInDb.Id != user.Id)
        {
            return new OperationResult<Unit, IReadOnlyCollection<string>>(new List<string> {"User with this EMail is already registered"});
        }

        var userEntity = _mapper.Map<UserEntity>(user);
        await _userRepository.Update(userEntity);
        
        return new OperationResult<Unit, IReadOnlyCollection<string>>(Unit.Instance);
    }

    public async Task<OperationResult<User, string>> AuthenticateUser(UserAuthenticationData userAuthenticationData)
    {
        var foundUser = await _userRepository.FindByEmail(userAuthenticationData.Email);

        if (foundUser is null)
        {
            return new OperationResult<User, string>($"There is no user with email {userAuthenticationData.Email}");
        }

        if (foundUser.Password != userAuthenticationData.Password)
        {
            return new OperationResult<User, string>("Incorrect password");
        }

        return new OperationResult<User, string>(_mapper.Map<User>(foundUser));
    }
    public void ObliterateUser(UserEntity user)
    {
    }
}

public interface IPhotoRepository
{

    public Task<OperationResult<string, string>> SavePhoto(int userId, Stream stream);
    public Task<OperationResult<Stream, string>> LoadPhoto(int userId);
}

public class PhotoRepository : IPhotoRepository
{
    private const string PathToPhoto = "wwwroot\\Images";

    public async Task<OperationResult<string, string>> SavePhoto(int userId, Stream stream)
    {
        var dir = new DirectoryInfo(PathToPhoto);
        if (!dir.Exists) dir.Create();

        string photoName = userId + ".jpg";

        stream.Seek(0, SeekOrigin.Begin);

        Bitmap image = new Bitmap(stream);
        var resizedImage = new Bitmap(image, new Size(300,300));
            
        string filePath = Path.Combine(PathToPhoto, photoName);
        resizedImage.Save(filePath);

        return new OperationResult<string, string>(result: filePath);
    }
    
    /// <summary>
    /// Resize the image to the specified width and height.
    /// </summary>
    /// <param name="image">The image to resize.</param>
    /// <param name="width">The width to resize to.</param>
    /// <param name="height">The height to resize to.</param>
    /// <returns>The resized image.</returns>
    private static Bitmap ResizeImage(Image image, int width, int height)
    {
        var destRect = new Rectangle(0, 0, width, height);
        var destImage = new Bitmap(width, height);

        destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        using (var graphics = Graphics.FromImage(destImage))
        {
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using (var wrapMode = new ImageAttributes())
            {
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width,image.Height, GraphicsUnit.Pixel, wrapMode);
            }
        }

        return destImage;
    }

    public async Task<OperationResult<Stream, string>> LoadPhoto(int userId)
    {
        FileInfo file = new FileInfo(Path.Combine(PathToPhoto, userId.ToString()));
        try
        {
            await using var fileStream = file.OpenRead();

            Stream stream = new MemoryStream();
            await fileStream.CopyToAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return new OperationResult<Stream, string>(stream);
        }
        catch (FileNotFoundException e)
        {
            throw new FileNotFoundException(file.FullName);
        }
    }
}
