using AutoMapper;
using Translator.Domain.Commands;
using Translator.Domain.Dtos;
using Translator.Domain.Models;

namespace Translator.Service.Dxos
{
    public class TranslationDxos : ITranslationDxos
    {
        private readonly IMapper _mapper;

        public TranslationDxos()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Translation, TranslationResponseDto>()
                    .ForMember(dst => dst.Result, opt => opt.MapFrom(src => src.Result))
                    .ForMember(dst => dst.Source, opt => opt.MapFrom(src => src.Source))
                    .ForMember(dst => dst.SourceLanguage, opt => opt.MapFrom(src => src.SourceLang))
                    .ForMember(dst => dst.TargetLanguage, opt => opt.MapFrom(src => src.TargetLang));

                cfg.CreateMap<TranslateRequestCommand, TranslationResponseDto>()
                    .ForMember(dst => dst.Source, opt => opt.MapFrom(src => src.Source))
                    .ForMember(dst => dst.SourceLanguage, opt => opt.MapFrom(src => src.SourceLanguage))
                    .ForMember(dst => dst.TargetLanguage, opt => opt.MapFrom(src => src.TargetLanguage));
            });

            _mapper = config.CreateMapper();
        }
        
        public TranslationResponseDto MapTranslationDto(Translation translation)
        {
            return _mapper.Map<Translation, TranslationResponseDto>(translation);
        }

        public TranslationResponseDto MapTranslationDto(TranslateRequestCommand command)
        {
            return _mapper.Map<TranslateRequestCommand, TranslationResponseDto>(command);
        }
    }
}