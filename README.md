Docker-compose file creator(not fully done( )
=============================
Программа для создания файла конфигурации системы .yml,.yaml. 
Используется версия 3.8, на нее ориентирована программа.
Программа по сути не проходила боевого крещения.

Быстрый старт
-----------
Для работы программы необходима библиотека GTK+ или просто gtk.
Установка Gtk на windows https://github.com/GtkSharp/GtkSharp/wiki/Installing-Gtk-on-Windows

Открыв приложение справа есть красивые ячейки сервисов, которые не несут какой-либо нагрузки кроме названия. Нажав на них они пол года перемещаются в левое окно(просто невозоможно пока сделать быстрее, скорее всего буду менять структуру программы).
Слева же находятся сервисы, из которых будет строиться ваш файл. Двойное нажатие на сервис открывает настройки сервиса, а также общие натсройки volumes,networks,secrets,volumes,configs. На данный мемент невозможно создать больше одного volumes, networks,secrets,volumes,configs.

Сейчас имеется возможность изменять простые поля(где нужно просто ввести текст), изменять Bool переменные, выбирать значения переменных через выпадающие списки, прописывать значнения для опций, которые могут потребовать нескольких значний. 
В некоторых полят требуется написание кавычек, но вам этого не нужно т.к. программа сама их поставит.
Для всех значений выпадающих списков и bool определены стандартные значения и не записываются при их не изменении.
При написании опций у драйвера в logging стои учитывать, что показываются настройки для всех драйверов и браться будут только те, что выбраны.
ALIASES и IPV4_ADDRESS, IPV6_ADDRESS не работают их нужно дописать вручную, если необходимо.
Нет в опциях deploy placement
Странно стали работать bool переменный, но потом вроде починились сами?)
Крашится при пустом списке сервисов))
Кнопка свой сервис не работает.

Программа будет исправляться уже ближе к августу. С середины июля или начала августа.
Планируется добавить настройки, как минимум версии docker-compose, под которую собирается файл. Изменить перенос сревиса из всех в выбранные. Добавить удаление сервиса. Сделать добавление собственного сервиса. Изменение его название.
