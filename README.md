Bu Proje Dahilinde ImemoryCache, IDistributedCache_Redis ve RedisExchangeApi Konuları Çalışılmıştır.

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------

Docker hub adresinden redis containerı kurmak için aşağıdaki komutu powerShall e gir

docker run -p 6380:6379 --name some-redis -d redis  //Kontainer içerisinde 6379 portunda ayağa kalkan servera ben container dan 6380 portundan bağlanmak istiyorum.

docker ps komutu ile hangi containerlarım ayağa kalmış görebilirim.

redis-cli -h localhost -p 6380 komutu ile ben localhost:6380 portundan serverema bağlanıp ping atıp çalışıp çalışmadığını kontrol edebilirim.

redis-cli -row derseniz Türkçe karakterli girişler de yapabilirsiniz. 
 

//******Redis Veri Tipleri Ve Komutları******//

1.String Veri Tipi
	-Set (key) (value)
	-Get (key)
	-GetRange key start end ( getrange name 0 2)(Belirli index aralığını getirir).
	-incr key (Eğer keyin tuttuğu value değeri integer bir değer ise değerini bir arttırır).
	-incrBy key increment (keyin tuttuğu value değerini increment kadar arttırır). 
	-decr key (increment'in tam tersi).
	-decrBy key decrement (incrby'ın tam tersi).
	-append key value (key değerine ait valueye value yi ekler).

2.List Veri Tipi
	-lpush value (Bir liste oluşturur ve o listenin soluna bir value yi ekler).
	-rpush value (Right push yukarıdakinin tersi).
	-lrange key start end (Listemizin solundan yani başından başlayarak belirli index aralığındaki liste elemanlarını döner).
	-lpop key (Listenin başındakini siler).
	-rpop key (listenin sonundakini siler).
	-lindex key index (listeden istenilen index değerini döner).

3.Set Veri Tipi(Set ile List veri tipi arasında şöyle bir farklılık var sette atılan veri unique olmalıdır ve rastgele kayıt atar, list buna bakmaz sonuna yada başına atar).
	-sadd key value
	-smembers key (okumak için)
	-srem key value (o value yi o key listesinden siler).

4.SortedSet Veri Tipi (Bir score değerine göre listeyi sıralar).
 	-zadd key score value (Aynı score ait veriler alt alta kaydedilir. Aynı veriyi bir daha kaydedemezssin).
	-zrange key start end (Index değerine göre listeleme).
	-zrem value (O value'yi listeden siler).
	-zrange key 0 -1 withscore (dersem scorları ile beraber getirir).	
	
5.Hash Veri Tipi (dictionary ye benziyor)
	-hmset listeAdı key value (eklemek için)
	-hget listeAdı key (get etmek için)
	-hdel listeAdı key (silmek için)
