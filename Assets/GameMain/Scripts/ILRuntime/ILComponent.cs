﻿//-----------------------------------------------------------------------
// <copyright file="ILComponent.cs" company="Codingworks Game Development">
//     Copyright (c) codingworks. All rights reserved.
// </copyright>
// <author> codingworks </author>
// <time> #CREATETIME# </time>
//-----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using ILRuntime.Runtime.Enviorment;
using System.IO;

namespace ILFramework
{
    public class ILComponent : GameFrameworkComponent
    {
        #region AppDomain接口
        public AppDomain _AppDomain
        {
            private set;
            get;
        }
        #endregion

        #region 属性
        #endregion

        protected override void Awake()
        {
            base.Awake();
            _AppDomain = new AppDomain();
        }
        
        #region 加载热更新
        public void LoadHotFixAssembly(byte[] _dllBytes, byte[] _pdbBytes)
        {
            if (_AppDomain == null)
                return;
            OnHotFixLoaded(_dllBytes, _pdbBytes);
        }

        void OnHotFixLoaded(byte[] _dllBytes,byte[] _pdbBytes)
        {
            if (_pdbBytes == null)
            {
                using (System.IO.MemoryStream fs = new MemoryStream(_dllBytes))
                {
                    _AppDomain.LoadAssembly(fs, null, new Mono.Cecil.Pdb.PdbReaderProvider());
                }
            }
            else
            {
                using (System.IO.MemoryStream fs = new MemoryStream(_dllBytes))
                {
                    using (System.IO.MemoryStream p = new MemoryStream(_pdbBytes))
                    {
                        _AppDomain.LoadAssembly(fs, p, new Mono.Cecil.Pdb.PdbReaderProvider());
                    }
                }
            }
            InitializeILRuntime();
        }

        void InitializeILRuntime()
        {

            RegisterDelegate();
            RegisterAdaptor();
        }

        //注册委托
        private void RegisterDelegate()
        {
            _AppDomain.DelegateManager.RegisterMethodDelegate<System.Object, GameFramework.Event.GameEventArgs>();
            _AppDomain.DelegateManager.RegisterDelegateConvertor<System.EventHandler<GameFramework.Event.GameEventArgs>>((act) =>
            {
                return new System.EventHandler<GameFramework.Event.GameEventArgs>((sender, e) =>
                {
                    ((System.Action<System.Object, GameFramework.Event.GameEventArgs>)act)(sender, e);
                });
            });

        }
        //注册继承适配器
        private void RegisterAdaptor()
        {
            //这里做一些ILRuntime的注册，HelloWorld示例暂时没有需要注册的
            _AppDomain.RegisterCrossBindingAdaptor(new ProcedureBaseAdaptor());
            _AppDomain.RegisterCrossBindingAdaptor(new IDisposableAdaptor());
            _AppDomain.RegisterCrossBindingAdaptor(new IDataRowAdaptor());
        }
        #endregion
    }
}
