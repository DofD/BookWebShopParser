namespace BookParser
{
    using System;
    using System.Configuration;
    using System.Linq;

    public class FactoryBookSaver : IFactoryBookSaver
    {
        /// <summary>
        /// Ключ в конфигурации отвечающий за имя длл брокера событий
        /// </summary>
        private const string BookSaverDllNameConfKey = "bookSaverDllName";

        /// <summary>
        /// Ключ в конфигурации отвечающий за имя класса брокера событий
        /// </summary>
        private const string BookSaverNameConfKey = "bookSaverName";

        public IBookSaver GetSaver()
        {
            var eventBrokerTypeName = this.GetSaverClassType();

            return string.IsNullOrEmpty(eventBrokerTypeName) ? new NoBookSaver() : this.CreateBookSaver(eventBrokerTypeName);
        }


        private IBookSaver CreateBookSaver(string saverBrokerTypeName)
        {
            // TODO Добавить логирование ошибок
            IBookSaver result = null;

            var saverType = System.Type.GetType(saverBrokerTypeName);
            try
            {
                if (saverType != null)
                {
                    result = (IBookSaver)Activator.CreateInstance(saverType);
                }
                else
                {
                    throw new ApplicationException("Тип " + saverBrokerTypeName + " не найтен");
                }
            }
            catch (MissingMethodException exception)
            {
                throw new ApplicationException("Не существует доступного конструктора" + saverType, exception);
            }
            catch (InvalidCastException exception)
            {
                throw new ApplicationException(saverType + "тип не реализует " + typeof(IBookSaver), exception);
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Ошибка создания: " + saverType, exception);
            }

            return result;
        }

        /// <summary>
        /// Получить имя класса брокера событий из конфигурации
        /// </summary>
        /// <returns>Имя класса брокера событий</returns>
        private string GetSaverClassType()
        {
            var eventBroker = ConfigurationManager.AppSettings.Keys.Cast<string>()
                .FirstOrDefault(k => FactoryBookSaver.BookSaverNameConfKey.ToLowerInvariant().Equals(k.ToLowerInvariant()));

            if (string.IsNullOrEmpty(eventBroker))
            {
                return string.Empty;
            }

            var eventBrokerName = ConfigurationManager.AppSettings[eventBroker];

            var eventBrokerDll = ConfigurationManager.AppSettings.Keys.Cast<string>()
                .FirstOrDefault(k => FactoryBookSaver.BookSaverDllNameConfKey.ToLowerInvariant().Equals(k.ToLowerInvariant()));

            return string.IsNullOrEmpty(eventBrokerDll)
                ? eventBrokerName
                : string.Format("{0}, {1}", eventBrokerName, ConfigurationManager.AppSettings[eventBrokerDll]);
        }
    }
}
