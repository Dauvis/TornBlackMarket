using AutoMapper;
using TornBlackMarket.Common.Enums;
using TornBlackMarket.Common.Interfaces;
using System.Security.Claims;
using TornBlackMarket.Common.DTO.Domain;
using TornBlackMarket.Common.DTO.External;

namespace TornBlackMarket.Logic.Services
{
    public class UserProfileService: IUserProfileService
    {
        private readonly ITornBlackMarketTokenUtil _tornBlackMarketTokenUtil;
        private readonly ITornApiKeyUtil _tornApiKeyTokenUtil;
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IMapper _mapper;
        private IUserProfileRepository? _userProfileRepository;

        public UserProfileService(ITornBlackMarketTokenUtil tornBlackMarketTokenUtil, ITornApiKeyUtil tornApiKeyTokenUtil, 
            IRepositoryFactory repositoryFactory, IMapper mapper) 
        {
            _tornBlackMarketTokenUtil = tornBlackMarketTokenUtil;
            _tornApiKeyTokenUtil = tornApiKeyTokenUtil;
            _repositoryFactory = repositoryFactory;
            _mapper = mapper;
        }

        protected IUserProfileRepository GetUserProfileRepository()
        {
            _userProfileRepository ??= _repositoryFactory.Create<IUserProfileRepository>()
                    ?? throw new InvalidOperationException($"Failed to instantiate repository: {nameof(IUserProfileRepository)}");

            return _userProfileRepository;
        }

        public async Task<AuthenticationResponseDTO> AuthenticateAsync(AuthenticationRequestDTO request)
        {
            try
            {
                var profileDto = await _tornApiKeyTokenUtil.ProfileDocumentForApiKeyAsync(request.TornApiKey);

                if (profileDto is null)
                {
                    return new AuthenticationResponseDTO()
                    {
                        ErrorCode = ErrorCodesType.AuthenticationFailed,
                        ErrorMessage = "Identity was not able to be verified: reason unknown"
                    };
                }
                else
                {
                    var jwt = _tornBlackMarketTokenUtil.GenerateToken(CreateClaimsPrincipal(profileDto));

                    return new AuthenticationResponseDTO()
                    {
                        WebToken = jwt,
                        UserId = profileDto.Id,
                        UserName = profileDto.Name
                    };
                }
            }
            catch (Exception e)
            {
                return new AuthenticationResponseDTO()
                {
                    ErrorCode = ErrorCodesType.AuthenticationFailed,
                    ErrorMessage = e.Message
                };
            }
        }

        private static ClaimsPrincipal CreateClaimsPrincipal(UserProfileDocumentDTO profileDto)
        {
            List<Claim> claims = [];
            claims.Add(new(ClaimTypes.NameIdentifier, profileDto.Id));
            claims.Add(new(ClaimTypes.Name, profileDto.Name));

            // TODO: Add user roles as claims, if any

            var identity = new ClaimsIdentity(claims, "Bearer");
            return new ClaimsPrincipal(identity);
        }

        public async Task<UserInfoDTO?> GetAsync(string userId)
        {
            var userRepository = GetUserProfileRepository();
            var userInfo = await userRepository.GetUserAsync(userId);

            return _mapper.Map<UserInfoDTO?>(userInfo);
        }

        public async Task<UserProfileDocumentDTO?> GetProfileAsync(string userId)
        {
            var userRepository = GetUserProfileRepository();
            var profile = await userRepository.GetAsync(userId);

            return _mapper.Map<UserProfileDocumentDTO?>(profile);
        }
    }
}
