﻿module models

open Alex75.Cryptocurrencies


type XrpWallet (address:string, destinationTag:string option) =
    member __.Address = address
    member __.DestinationTag = destinationTag


[<AbstractClass>]
type Response (isSuccess:bool, error:string) = 
    member __.IsSuccess = isSuccess
    member __.Error = error

type TickerResponse (isSuccess:bool, error:string, ticker:Option<Ticker>) = //{ IsSuccess:bool; Error:string; Ticker:Ticker }
    inherit Response( isSuccess, error) 
    member __.Ticker = ticker

type Ticker_price(bidPrice:decimal, askPrice:decimal) =
    member __.Bid = bidPrice
    member __.Ask = askPrice
    
    
(*
{
  "symbol": "LTCBTC",
  "bidPrice": "4.00000000",
  "bidQty": "431.00000000",
  "askPrice": "4.00000200",
  "askQty": "9.00000000"
}    
*)

// {"code":-1121,"msg":"Invalid symbol."}

// Binance API response
type Ticker_24h(code:string, msg:string,
                lastPrice:decimal, bidPrice:decimal, askPrice:decimal, 
                openPrice:decimal, highPrice:decimal, lowPrice:decimal) =

    member __.IsSuccess = (code = null)
    member __.Error = msg

    member __.LastPrice = lastPrice
    member __.BidPrice = bidPrice
    member __.AskPrice = askPrice
    member __.OpenPrice = openPrice
    member __.HighPrice = highPrice
    member __.LowPrice = lowPrice

    member __.ToResponse pair = 

        match __.IsSuccess with 
        //| true -> TickerResponse(true, null, Some(Ticker(pair, bidPrice, askPrice, Some(lowPrice), Some(highPrice), Some(lastPrice))))
        | false -> TickerResponse(false, __.Error, None)
        | true -> 
            let ticker = Ticker(pair, bidPrice, askPrice, Some(lowPrice), Some(highPrice), Some(lastPrice))
            TickerResponse(true, null, Some(ticker))


(*
{
  "symbol": "BNBBTC",
  "priceChange": "-94.99999800",
  "priceChangePercent": "-95.960",
  "weightedAvgPrice": "0.29628482",
  "prevClosePrice": "0.10002000",
  "lastPrice": "4.00000200",
  "lastQty": "200.00000000",
  "bidPrice": "4.00000000",
  "askPrice": "4.00000200",
  "openPrice": "99.00000000",
  "highPrice": "100.00000000",
  "lowPrice": "0.10000000",
  "volume": "8913.30000000",
  "quoteVolume": "15.30000000",
  "openTime": 1499783499040,
  "closeTime": 1499869899040,
  "firstId": 28385,   // First tradeId
  "lastId": 28460,    // Last tradeId
  "count": 76         // Trade count
}
*)


type CreateOrderResponse(isSuccess:bool, error:string, orderId:int64, price:decimal) =
    member this.IsSuccess = isSuccess
    member this.Error = error

    member this.Id = orderId
    member this.Price = price


// Binance API response
type internal BinanceOrderFullResponse(orderId:int64, price:decimal) =
    //member this.Id = orderId
    //member this.Price = price

    member __.ToResponse () =

        CreateOrderResponse(true, null, orderId, price)


type WithdrawResponse(msg:string, success:bool, id:string) =     
    member __.IsSuccess = success
    member __.Message = msg
    member __.Id = id
