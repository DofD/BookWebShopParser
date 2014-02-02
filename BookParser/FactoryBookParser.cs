namespace BookParser
{
    using System;
    using System.Configuration;
    using System.Linq;

    public class FactoryBookParser : IFactoryBookParser
    {
        /// <summary>
        /// Ключ в конфигурации отвечающий за имя длл брокера событий
        /// </summary>
        private const string BookParserDllNameConfKey = "bookParserDllName";

        /// <summary>
        /// Ключ в конфигурации отвечающий за имя класса брокера событий
        /// </summary>
        private const string BookParserNameConfKey = "bookParserName";

        public IBookParser GetParser()
        {
            var eventBrokerTypeName = this.GetParserClassType();

            return string.IsNullOrEmpty(eventBrokerTypeName) ? new NoBookParser() : this.CreateBookParser(eventBrokerTypeName);
        }


        private IBookParser CreateBookParser(string eventBrokerTypeName)
        {
            // TODO Добавить логирование ошибок
            IBookParser result = null;

            var eventBrokerType = System.Type.GetType(eventBrokerTypeName);
            try
            {
                if (eventBrokerType != null)
                {
                    result = (IBookParser)Activator.CreateInstance(eventBrokerType);
                }
                else
                {
                    throw new ApplicationException("Тип " + eventBrokerTypeName + " не найтен");
                }
            }
            catch (MissingMethodException exception)
            {
                throw new ApplicationException("Не существует доступного конструктора" + eventBrokerType, exception);
            }
            catch (InvalidCastException exception)
            {
                throw new ApplicationException(eventBrokerType + "тип не реализует " + typeof(IBookParser), exception);
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Ошибка создания: " + eventBrokerType, exception);
            }

            return result;
        }

        /// <summary>
        /// Получить имя класса брокера событий из конфигурации
        /// </summary>
        /// <returns>Имя класса брокера событий</returns>
        private string GetParserClassType()
        {
            var eventBroker = ConfigurationManager.AppSettings.Keys.Cast<string>()
                .FirstOrDefault(k => FactoryBookParser.BookParserNameConfKey.ToLowerInvariant().Equals(k.ToLowerInvariant()));

            if (string.IsNullOrEmpty(eventBroker))
            {
                return string.Empty;
            }

            var eventBrokerName = ConfigurationManager.AppSettings[eventBroker];

            var eventBrokerDll = ConfigurationManager.AppSettings.Keys.Cast<string>()
                .FirstOrDefault(k => FactoryBookParser.BookParserDllNameConfKey.ToLowerInvariant().Equals(k.ToLowerInvariant()));

            return string.IsNullOrEmpty(eventBrokerDll)
                ? eventBrokerName
                : string.Format("{0}, {1}", eventBrokerName, ConfigurationManager.AppSettings[eventBrokerDll]);
        }
    }
}
