import React, { useState, useEffect } from 'react';
import authService from '../api-authorization/AuthorizeService';
import { Modal, Form, ModalHeader, ModalBody } from 'reactstrap';

export default function ContaForm(props) {
    return (
        <div>
            <Modal isOpen={props.showFormConta} toggle={() => props.setShowFormConta(false)}>
                <ModalHeader toggle={() => props.setShowFormConta(false)}>Nova Conta</ModalHeader>
            </Modal>
        </div>
    )
}