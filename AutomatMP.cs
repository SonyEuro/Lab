﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Lab
{

    /// <summary>
    /// Автомат с магазинной памятью.
    /// </summary>
    class AutomatMP
    {
        /// <summary>
        /// Состояния автомата.
        /// </summary>
        enum state { q0, q1, q2 };
        /// <summary>
        /// Действия автомата.
        /// </summary>
        enum action { push, pop, confirm };
        /// <summary>
        /// Правила автомата.
        /// </summary>
        object[,] rules;
        /// <summary>
        /// Текущее состояние автомата.
        /// </summary>
        const int st0 = 0;
        /// <summary>
        /// Входной символ в автомат.
        /// </summary>
        const int inCh = 1;
        /// <summary>
        /// Верхушка стека автомата.
        /// </summary>
        const int stckTop = 2;
        /// <summary>
        /// Следущее состояние автомата.
        /// </summary>
        const int st1 = 3;
        /// <summary>
        /// Действие автомата.
        /// </summary>
        const int doing = 4;
        /// <summary>
        /// Текущее состояние автомата.
        /// </summary>
        state st;
        /// <summary>
        /// Таблица функицй переходов.
        /// </summary>
        public List<String[]> JumpFunction = new List<string[]>();

        /// <summary>
        /// Символ конца стека. А так же конца ленты.
        /// </summary>
        const char endStack = '&';

        Stack stack = new Stack();

        Tape tape;

        public bool getConfirmation()
        {
            return tape.confirmation;
        }

        /// <summary>
        /// Конструктор.
        /// Добавляет в стек конце стека. Формирует правила работы. Устанавливает текущее состояние в начальное, то есть в q0.
        /// </summary>
        public AutomatMP()
        {
            //добавляем символ конца в стек
            //stack.Add(endStack);
            createRules();
            st = state.q0;
        }
        /// <summary>
        /// Создание правил работы распознавания слов языка
        /// </summary>
        private void createRules()
        {
            rules = new object[5, 5] {
                {state.q0,  '0',        endStack,   state.q1,   action.push},//typeof(AutomatMP).GetMethod("push").MethodHandle.GetFunctionPointer();
                {state.q1,  '0',        '0',        state.q1,   action.push},
                {state.q1,  '1',        '0',        state.q2,   action.pop},
                {state.q2,  '1',        '0',        state.q2,   action.pop},
                {state.q2,  endStack,   endStack,   state.q0,   action.confirm},
            };
        }
        /// <summary>
        /// Возвращение к начальному состоянию.
        /// </summary>
        private void SetNullState()
        {
            JumpFunction.Clear();
            stack = new Stack();
            //stack.Clear();
            //stack.Add(endStack);
            st = state.q0;
            //confirmation = false;
        }
        
        /// <summary>
        /// Запуск автомата с входной строкой.
        /// </summary>
        /// 
        public void Run(string str)
        {
            SetNullState();
            tape = new Tape(str);
            for (int i = 0; i < tape.getTape().Length; i++)
            {
                JumpFunction.Add(new String[] { new string(stack.getStack().ToArray()), st.ToString(), tape.getTape().Substring(i,tape.getTape().Length-i)});
                for (int m = 0; m < 5; //rules.Rank
                    m++)
                {
                    if ((st == (state)rules[m, st0]) &&
                        (tape.getTape(i) == (char)rules[m, inCh]) &&
                        (stack.topStack() == (char)rules[m, stckTop]))
                    {
                        st = (state)rules[m, st1];
                        //(IntPtr)rules[m, doing]('0');
                        switch (rules[m, doing])
                        {
                            case action.push:
                                stack.push('0');
                                break;
                            case action.pop:
                                stack.pop();
                                break;
                            case action.confirm:
                                tape.confirm();
                                break;
                        }
                        break;
                    }
                    if (m == 4)
                        return;
                }
            }
        }
    }
}