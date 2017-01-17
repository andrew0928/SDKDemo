





# SDK ToDo:

[X] security: api access token  

[ ] data transaction

[X] SDK 要能簡化 app 呼叫 api 的程序  
> 貼近使用的 programming language 使用慣例
> 例如芬頁的機制要用 IEnumerable 包裝

[ ] SDK 要能進行效能優化 (ex: cache)

[X] SDK (client side) interface
> 確保 app / sdk 的向前相容性。  
> 若 SDK 有新版本，要保證 app 程式，只需更換新版 SDK DLL 即可。
> 不用修改 app code 就要能繼續使用

[X] API (server side) interface
> 確保 sdk / api 的向前相容性。
> 若 server api 有異動，要保證 app 只要更新到支援範圍內的 SDK，都要能正常使用

[ ] API retry 相容性 (safe & idempotent)
> (同樣的 API 重複呼叫多次，不會影響結果)

[X] API 必須遵照 HTTP status code

## Reference
https://tw.twincl.com/programming/*641y
> RESTful API設計的第一步，是充份了解常用的HTTP動詞。一些API設計的選擇容或見人見智，用錯HTTP動詞就不好了。
> 
> GET: 讀取資源 (safe & idempotent)  
> PUT: 替換資源 (idempotent)  
> DELETE: 刪除資源 (idempotent)  
> POST: 新增資源；也作為萬用動詞，處理其它要求  
> PATCH: 更新資源部份內容  
> HEAD: 類似GET，但只回傳HTTP header (safe & idempotent)  
> 其它還有一些較少用到的，可參考Wikipedia: Hypertext Transfer Protocol  
> 以上「safe」是指該操作不會改變伺服器端的資源狀態（而且結果可以被cache）；「idempotent」是指該操作不管做1遍或做n遍，都會得到同樣的資源狀態結果（但不一定得到同樣的回傳值，例如第2次DELETE請求可能回傳404），因此client端可以放心retry。  

http://www.restapitutorial.com/lessons/idempotency.html

swagger
https://docs.asp.net/en/latest/tutorials/web-api-help-pages-using-swagger.html

ASP.NET Web API 文件產生器 - 使用 Swagger
http://kevintsengtw.blogspot.tw/2015/12/aspnet-web-api-swagger.html

ASP.NET WEB API 文件產生器(2) - SWAGGER
http://blog.kkbruce.net/2015/04/aspnet-web-api-2-swagger.html#.V_uwBCh96UM

Swashbuckle - Swagger for Web Api 顯示內容的調整
http://kevintsengtw.blogspot.tw/2015/12/swashbuckle-swagger-for-web-api.html

# DATA


# REFERENCES

* [Azure API Apps](https://azure.microsoft.com/en-us/documentation/articles/app-service-api-apps-why-best-platform/)
* [Swagger API Metadata](http://swagger.io/)
* [govdata](http://data.gov.tw/node/6006)