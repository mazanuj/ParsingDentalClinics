﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ParsingDentalClinics.Config;
using ParsingDentalClinics.Interfaces;
using ParsingDentalClinics.Utils;

namespace ParsingDentalClinics.Sites
{
    using System.Threading.Tasks;

    internal class VseKz
    {
        public static async Task<IEnumerable<InfoHolder>> GetInfo()
        {
            return await Task.Run(
                () =>
                {
                    const string data = @"
                ASTRUM;пр.Абая, 95/1;299227
                DENT AURA;ул.Ауэзова, 4;214345
                DENT LUX;пр.Абая, 28;321010, 327873
                DENTA-L;ул.Акжайык, 5;392025
                DENTA-L;мкрн.5, д.22;368105
                DIAMOND STAR;пр.Момышулы, 4;283200
                DOCTOR DENT;пр.Победы, 21;230307, 230674
                LEGEARTIS;ул.Кенесары, 50;211823, 217017
                MASTER DENT;ул.Кенесары, 50;212582, 215486
                MEDICUS DENT;пр.Республики, 29;328606
                NC DENT, ТОО;г.Астана, ул.Акжайык, 21, оф.18;387128
                NOVA DENT;г.Астана, пр.Богенбай батыра, 36 А;322086
                NS DENT;г.Астана, пр.Абылай хана, 8;357596
                Азат-Дентес;г.Астана, ул.Есенберлина, 4;392605
                Азия Стом;г.Астана, пр.Абая, 57;212161
                Айра;г.Астана, пр.Республики, 5;217009
                Ак-Дент;г.Астана, пр.Победы, 89 А;384749
                Акмаржан;г.Астана, ул.Акжайык, 28;385317
                Голливудская улыбка;г.Астана, бул.Академика Скрябина, 6;317234, 317244
                Дента КВК;г.Астана, ул.Джангильдина, 7;321324, 323391
                Дента Сервис А;г.Астана, ул.Бейбитшилик, 40;319458
                Дом здоровья, ТОО;г.Астана, ул.Пушкина, 155;397618, 397447
                ЗарДан;г.Астана, ул.Кошкарбаева, 36;215672
                Интерстом, ТОО;г.Астана, ул.Бараева, 11, оф.102;211010
                Кенес-Дент;г.Астана, ул.Бейбитшилик, 29;316371
                МДС Сервис Плюс, медицинский центр;г.Астана, мкрн.Молодёжный, 31;224080
                Медицина А и К Дент;г.Астана, мкрн.Самал, 2;222537
                МейIрIм;г.Астана, Левобережье, корпус 4;974005, 974012
                Омега-Дент;г.Астана, ул.Сейфуллина, 29;328070
                Омега-Дент;г.Астана, пр.Богенбай батыра, 50;319026
                Ортостоматолог;г.Астана, ул.Бейбитшилик, 60;398454
                ПрезиDENT;г.Астана, мкрн.Молодёжный, 18;229265
                Синтез, ТОО;г.Астана, ул.9 мая, 30;397764, 315960
                Стомадент, стоматологический кабинет;г.Астана, пр.Победы, 73;394243
                Стоматолог, ТОО;г.Астана, мкрн.1, д.20/1;361915, 361637
                Стоматологическая поликлиника;г.Астана, ул.Кенесары, 264;370398, 370392
                Стоматологическая поликлиника;г.Астана, мкрн.Молодёжный, 42;299783, 299739
                Стомсервис, стоматологический кабинет;г.Астана, мкрн.2, д.20;364838
                Хаким, ТОО;г.Астана, пр.Абылай хана, 16/1;356684, 356080
                Шахар, стоматологический кабинет;г.Астана, ул.Бейбитшилик, 58;397613
                Шын-Зар, ТОО;г.Астана, мкрн.5, д.1, оф.1;357415
                Мукай, ИП;г.Астана, ул.Желтоксан, 37;981209
                Стоматологическая клиника (ИП Омарова);г.Астана, мкрн.Аль-Фараби, 14/1;232868
                Стоматологическая поликлиника;г.Астана, мкрн.Аль-Фараби, 77;232469
                Стоматология (ИП Балгазарова);г.Астана, пр.Победы, 24;322005
                Стоматология (ИП Оноприенко);г.Астана, пр.Победы, 63/1;934263
                Стоматология (ИП Сагиев);г.Астана, пр.Богенбай батыра, 31, кв.52;316600
            ";

                    var clinics =
                        Regex.Replace(data, @"\r", "")
                            .Split('\n')
                            .Select(RegExHelper.RegExpression)
                            .Where(x => !string.IsNullOrEmpty(x));

                    return new List<InfoHolder>(clinics.Select(clinic =>
                    {
                        var rows = clinic.Split(';');
                        return new InfoHolder
                        {
                            Site = SiteEnum.VseKz,
                            Country = CountryEnum.Kazakhstan,
                            ClinicName = rows[0],
                            Address = rows[1],
                            Phone = rows[2]
                        };
                    }));
                });
        }
    }
}