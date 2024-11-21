using AutoMapper;
using TornBlackMarket.Common.Enums;
using TornBlackMarket.Common.Interfaces;
using System.Security.Claims;
using TornBlackMarket.Common.DTO.Domain;
using TornBlackMarket.Common.DTO.External;

namespace TornBlackMarket.Logic.Services
{
    public class ProfileService: IProfileService
    {
        private readonly ITornBlackMarketTokenUtil _tornBlackMarketTokenUtil;
        private readonly ITornApiKeyUtil _tornApiKeyTokenUtil;
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IMapper _mapper;
        private IProfileRepository? _userProfileRepository;

        public ProfileService(ITornBlackMarketTokenUtil tornBlackMarketTokenUtil, ITornApiKeyUtil tornApiKeyTokenUtil, 
            IRepositoryFactory repositoryFactory, IMapper mapper) 
        {
            _tornBlackMarketTokenUtil = tornBlackMarketTokenUtil;
            _tornApiKeyTokenUtil = tornApiKeyTokenUtil;
            _repositoryFactory = repositoryFactory;
            _mapper = mapper;
        }

        protected IProfileRepository GetProfileRepository()
        {
            _userProfileRepository ??= _repositoryFactory.Create<IProfileRepository>()
                    ?? throw new InvalidOperationException($"Failed to instantiate repository: {nameof(IProfileRepository)}");

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
                        PlayerId = profileDto.Id,
                        PlayerName = profileDto.Name
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

        private static ClaimsPrincipal CreateClaimsPrincipal(ProfileDocumentDTO profileDto)
        {
            List<Claim> claims = [];
            claims.Add(new(ClaimTypes.NameIdentifier, profileDto.Id));
            claims.Add(new(ClaimTypes.Name, profileDto.Name));            

            // TODO: Add user roles as claims, if any

            var identity = new ClaimsIdentity(claims, "Bearer");
            return new ClaimsPrincipal(identity);
        }

        public async Task<ProfileDocumentDTO?> GetAsync(string profileId)
        {
            var userRepository = GetProfileRepository();
            var profile = await userRepository.GetAsync(profileId);

            return _mapper.Map<ProfileDocumentDTO?>(profile);
        }

        public async Task<bool> InvalidateTokensAsync(string profileId)
        {
            var repository = GetProfileRepository();
            var profile = await repository.GetAsync(profileId);

            if (profile is not null)
            {
                profile.TokenInvalidDateTime = DateTimeOffset.Now;
                return await repository.UpdateAsync(profile);
            }

            return false;
        }
    }
}
