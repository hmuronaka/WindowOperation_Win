using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowOperation
{
    public class RequestTokenizer
    {
        public enum TokenType
        {
            EMPTY,
            COMMAND,
            NUMBER,
            DATA,
            INVALID_TOKEN,
            EOF
        };

        /// <summary>
        /// 受信文字列.
        /// </summary>
        public String request
        {
            private set;
            get;
        }

        /// <summary>
        /// 最後に読み取った文字列.
        /// </summary>
        public String token
        {
            private set;
            get;
        }
        public TokenType tokenType
        {
            private set;
            get;
        }

        /// <summary>
        /// 現在位置.
        /// </summary>
        public int position
        {
            private set;
            get;
        }

        public RequestTokenizer(String request)
        {
            this.request = request;
            this.position = 0;
            this.token = "";
            this.tokenType = TokenType.EMPTY;
        }

        /// <summary>
        /// 次のトークンを読み込む.
        /// </summary>
        /// <returns></returns>
        public bool nextToken()
        {
            // 末尾.
            if (this.position >= request.Length)
            {
                this.token = "";
                this.tokenType = TokenType.EOF;
                return false;
            }

            // データの格納先.
            StringBuilder tokenBuilder = new StringBuilder();

            if (nowChar() == '@')
            {
                this.tokenType = TokenType.COMMAND;
                tokenBuilder.Append(nowChar());
                nextChar();

            }
            else if (nowChar() == '#')
            {
                this.tokenType = TokenType.NUMBER;
                nextChar();
            }
            else
            {
                this.tokenType = TokenType.DATA;
            }

            bool isEnd = false;
            while (this.position < request.Length && !isEnd)
            {
                char nowCh = nowChar();
                if (nowCh == '\\')
                {
                    char peekCh = peekChar();
                    switch (peekCh)
                    {
                        case '\\':
                        case ';':
                        case '#':
                        case '@':
                            tokenBuilder.Append(peekCh);
                            nextChar();
                            break;
                        default:
                            tokenBuilder.Append(nowCh);
                            break;
                    }
                }
                else if (nowCh != ';')
                {
                    tokenBuilder.Append(nowCh);
                }
                else
                {
                    isEnd = true;
                }
                nextChar();
            }

            // トークンは必ず;で終わるので、そうでなければ不正とする.
            if (!isEnd)
            {
                this.tokenType = TokenType.INVALID_TOKEN;
            }
            this.token = tokenBuilder.ToString();
            return true;
        }

        private char nowChar()
        {
            if (this.position < this.request.Length)
            {
                return this.request[this.position];
            }
            else
            {
                return '\0';
            }
        }

        private char peekChar()
        {
            this.position++;
            char ch = nowChar();
            this.position--;
            return ch;
        }

        private char nextChar()
        {
            this.position++;
            return nowChar();
        }
    }
}
