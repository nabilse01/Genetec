// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk;
using Genetec.Sdk.Workspace.Services;
using OfficerMapObject.MapObjects.Officers;
using OfficerMapObject.Panels;
using System.Threading;

namespace OfficerMapObject
{
    public sealed class Module : Genetec.Sdk.Workspace.Modules.Module
    {
        #region Private Fields

        private AddOfficerContextualAction m_caAddOfficer;
        private addOfficer m_addOfficer;
        private IContextualActionsService m_contextualActionsService;
        private LayerDescriptor m_layerDescriptor;
        private IMapService m_mapService;
        private OfficerMapObjectBuilder m_officerMapObjectBuilder;
        private OfficerMapObjectBuilder1 m_officerMapObjectBuilder1;

        private OfficerMapObjectProvider m_officerMapObjectProvider;
        private OfficerMapObjectProvider1 m_officerMapObjectProvider1;


        private OfficersMapPanelBuilder m_officersMapPanelBuilder;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Loads the module in the workspace and register it's workspace extensions and shared components
        /// </summary>
        public override void Load()
        {
            if (Workspace.ApplicationType != ApplicationType.SecurityDesk) return;

            m_contextualActionsService = Workspace.Services.Get<IContextualActionsService>();

            if (m_contextualActionsService != null)
            {
                m_caAddOfficer = new AddOfficerContextualAction();
                m_caAddOfficer.Initialize(Workspace);
                m_contextualActionsService.Register(m_caAddOfficer);
                                        
            }
            m_officerMapObjectBuilder = new OfficerMapObjectBuilder();
            m_officerMapObjectBuilder.Initialize(Workspace);
            Workspace.Components.Register(m_officerMapObjectBuilder);

            m_officerMapObjectBuilder1 = new OfficerMapObjectBuilder1();
            m_officerMapObjectBuilder1.Initialize(Workspace);
            Workspace.Components.Register(m_officerMapObjectBuilder1);

            m_officerMapObjectProvider = new OfficerMapObjectProvider(Workspace);
            Workspace.Components.Register(m_officerMapObjectProvider);

            m_officerMapObjectProvider1 = new OfficerMapObjectProvider1(Workspace);
            Workspace.Components.Register(m_officerMapObjectProvider1);

            m_mapService = Workspace.Services.Get<IMapService>();

            m_layerDescriptor =
                new LayerDescriptor(MapObjects.Officers.OfficerMapObject.OfficerLayerId, OfficerMapObjectView.OfficerLayerName);
            m_mapService?.RegisterLayer(m_layerDescriptor);

            m_officersMapPanelBuilder = new OfficersMapPanelBuilder();
            m_officersMapPanelBuilder.Initialize(Workspace);
            Workspace.Components.Register(m_officersMapPanelBuilder);
            m_addOfficer = new addOfficer();
            m_addOfficer.add();
        }
        

        /// <summary>
        /// Unloads the module in the workspace by unregistering it's workspace extensions and shared components
        /// </summary>
        public override void Unload()
        {
            m_contextualActionsService = Workspace.Services.Get<IContextualActionsService>();

            m_contextualActionsService?.Unregister(m_caAddOfficer);
            Workspace.Components.Unregister(m_officerMapObjectBuilder);

            if (m_officerMapObjectProvider != null)
            {
                Workspace.Components.Unregister(m_officerMapObjectProvider);
                m_officerMapObjectProvider = null;
            }

            m_mapService?.UnregisterLayer(m_layerDescriptor);

            if (m_officersMapPanelBuilder == null) return;
            Workspace.Components.Unregister(m_officersMapPanelBuilder);
            m_officersMapPanelBuilder = null;
        }

        #endregion Public Methods
    }
}